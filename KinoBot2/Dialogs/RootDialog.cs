﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using KinoBot2.Parsers;
using KinoBot2.DAO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using KinoBot2.Forms;
using Microsoft.Bot.Builder.FormFlow;

namespace KinoBot2.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<ChooseFilmForm>
    {
        public static string BACK_OPERATION_TEXT = "Произошел выход. Наберите 'Фильмы на неделе', чтобы получить список фильмов";
        public static List<WeekResponse> completeWeekResponse = new List<WeekResponse>();
        public static Transformer t = new Transformer();
        public static string movieName;
        public static string movieDate;
        public static Dictionary<string, List<string>> map;
        public static string movieFormat;
        public Task StartAsync(IDialogContext context)
        
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            JsonParser jp = new JsonParser();
            var activity = await result as Activity;
            StringBuilder sb = new StringBuilder();

            if (activity.Text.Contains("Фильмы на неделе"))
            {
                
                List<WeekResponse> weekResponse = jp.getWeekResponse();
                List<MoviesResponse> moviesResponse = jp.getMoviesResponse();
                List<HallsResponse> hallsResponse = jp.getHallsResponse();
                
                completeWeekResponse = t.uniteResponses(weekResponse, moviesResponse, hallsResponse);
                ChooseFilmForm.username = activity.From.Name;
                ChooseFilmForm.moviesList = t.getAllMovies(completeWeekResponse);
                context.Call(ChooseFilmForm.BuildMoviesDialog(FormOptions.PromptInStart), FormComplete);
            }
            else
            {
                await context.PostAsync("Дорогой(ая) "+activity.From.Name+"! Такой команды в моем списке нет. Напишите 'Фильмы на неделе', чтобы получить список фильмов");
            }
            

            //await context.PostAsync(sb.ToString());
            //context.Wait(MessageReceivedAsync);
        }

        private async Task FormComplete(IDialogContext context, IAwaitable<ChooseFilmForm> result)
        {
            try
            {
                var form = await result;
                if (form != null)
                {
                    movieName = form.MovieName;
                    ChooseDateForm.datesList = t.getMovieDates(completeWeekResponse, movieName);
                    context.Call(ChooseDateForm.BuildDatesDialog(FormOptions.PromptInStart), DatesFormComplete);
                }
                else
                {
                    await context.PostAsync("Form returned empty response! Type anything to restart it.");
                }
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync(BACK_OPERATION_TEXT);
            }

            //context.Wait(MessageReceivedAsync);
        }

        private async Task DatesFormComplete(IDialogContext context, IAwaitable<ChooseDateForm> result)
        {
            try
            {
                var form = await result;
                if (form != null)
                {
                    movieDate = form.Date;
                    map = t.getMovieTimes(completeWeekResponse, movieName, movieDate);
                    ChooseMovieFormatForm.formatList = map.Keys.ToList();
                    context.Call(ChooseMovieFormatForm.BuildDatesDialog(FormOptions.PromptInStart), FormatsFormComplete);

                }
                else
                {
                    await context.PostAsync("Form returned empty response! Type anything to restart it.");
                }
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync(BACK_OPERATION_TEXT);
            }

        }

        private async Task FormatsFormComplete(IDialogContext context, IAwaitable<ChooseMovieFormatForm> result)
        {
            try
            {
                var form = await result;
                if (form != null)
                {
                    movieFormat = form.Format;
                    List<string> movieTimes = new List<string>();
                    map.TryGetValue(movieFormat, out movieTimes);
                    ChooseMovieTime.timesList = movieTimes;
                    context.Call(ChooseMovieTime.BuildDatesDialog(FormOptions.PromptInStart), TimesFormComplete);

                }
                else
                {
                    await context.PostAsync("Form returned empty response! Type anything to restart it.");
                }
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync(BACK_OPERATION_TEXT);
            }

            
        }

        private async Task TimesFormComplete(IDialogContext context, IAwaitable<ChooseMovieTime> result)
        {
            try
            {
                var form = await result;
                if (form != null)
                {
                    string link = t.getSeanse(completeWeekResponse, movieName, movieDate, form.Time);
                    await context.PostAsync(link);

                }
                else
                {
                    await context.PostAsync("Form returned empty response! Type anything to restart it.");
                }
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync(BACK_OPERATION_TEXT);
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}
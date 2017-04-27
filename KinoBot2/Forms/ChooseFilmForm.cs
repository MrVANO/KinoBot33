using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KinoBot2.Forms
{
    [Serializable]
    public class ChooseFilmForm
    {
        public static List<string> moviesList = new List<string>() ;
        public static string username { get; set; }

        [Prompt("{||}")]
        //[Template(TemplateUsage.Feedback, "Сейчас покажу даты показа")]
        [Template(TemplateUsage.NotUnderstood, "Вы ввели значение \"{0}\", которого нет на последней форме")]
        public string MovieName { get; set; }

        public static IForm<ChooseFilmForm> BuildMoviesForm()
        {
            IFormBuilder<ChooseFilmForm> builder = new FormBuilder<ChooseFilmForm>()
                    .Message("Вот фильмы идущие на этой неделе:")
                    .Field(new FieldReflector<ChooseFilmForm>(nameof(MovieName)
                    )
                            .SetType(null)
                            .SetDefine((state, field) =>
                            {
                                foreach (var prod in GetMovies())
                                    field
                                        .AddDescription(prod, prod)
                                        .AddTerms(prod, prod);
                                var exit = "Выход";
                                field.AddDescription(exit, exit).AddTerms(exit, exit);
                                return Task.FromResult(true);
                            })
                            .SetActive((state) =>
                            {
                                string s = state.MovieName;
                                return true;
                            })
                            .SetNext((value, state) =>
                            {
                                var nextStep = new NextStep();
                                return nextStep;
                            })
                            .SetValidate(
                                validate: async (state, response) =>
                                {
                                    var result = new ValidateResult { IsValid = false, Value = response };
                                    var movie = (response as string);
                                    foreach (var movieName in GetMovies())
                                    {
                                        if (movie.Contains(movieName))
                                        {
                                            result.Feedback = "Запрос корректен";
                                            result.IsValid = true;
                                            break;
                                        }
                                        if (movie.Contains("Выход")){
                                            result.Feedback = "Произвожу выход...";
                                            result.IsValid = true;
                                            break;
                                        }
                                    }
                                    if (!result.IsValid)
                                    {
                                        result.Feedback = "Случилась непредвиденная ошибка!!! Производится выход";
                                        throw new OperationCanceledException();
                                    }
                                    return result;
                                })
                            )
                    .AddRemainingFields();

            return builder.Build(); 
        }

        public static IFormDialog<ChooseFilmForm> BuildMoviesDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildMoviesForm, options);
        }

        static List<string> GetMovies()
        {
            return moviesList;
        }
    }
}
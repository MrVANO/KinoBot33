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
    public class ChooseMovieTime
    {
        public static List<string> timesList = new List<string>();

        [Prompt("Сеансы {||}")]
        [Template(TemplateUsage.NotUnderstood, "Вы ввели значение \"{0}\", которого нет на последней форме. Выберите значение из последней формы.")]
        public string Time { get; set; }

        public static IForm<ChooseMovieTime> BuildDatesForm()
        {

            return new FormBuilder<ChooseMovieTime>()
                    .Field(new FieldReflector<ChooseMovieTime>(nameof(Time))
                            .SetType(null)
                            .SetDefine((state, field) =>
                            {
                                foreach (var prod in GetTimes())
                                    field
                                        .AddDescription(prod, prod)
                                        .AddTerms(prod, prod);
                                var exit = "Выход";
                                field.AddDescription(exit, exit).AddTerms(exit, exit);
                                return Task.FromResult(true);
                            })
                            .SetValidate(
                                validate: async (state, response) =>
                                {
                                    var result = new ValidateResult { IsValid = false, Value = response };
                                    var movie = (response as string);
                                    foreach (var movieName in GetTimes())
                                    {
                                        if (movie.Contains(movieName))
                                        {
                                            result.Feedback = "Запрос корректен";
                                            result.IsValid = true;
                                            break;
                                        }
                                        if (movie.Contains("Выход"))
                                        {
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
                    .AddRemainingFields()
                    .Build();
        }

        public static IFormDialog<ChooseMovieTime> BuildDatesDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildDatesForm, options);
        }

        static List<string> GetTimes()
        {
            return timesList;
        }

    }
}
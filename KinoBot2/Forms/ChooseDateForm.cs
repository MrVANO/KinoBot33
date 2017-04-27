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
    public class ChooseDateForm
    {
        public static List<string> datesList = new List<string>();

        [Prompt("Даты показа {||}")]
        [Template(TemplateUsage.NotUnderstood, "Вы ввели значение \"{0}\", которого нет на последней форме. Выберите значение из последней формы.")]
        public string Date { get; set; }

        public static IForm<ChooseDateForm> BuildDatesForm()
        {

            return new FormBuilder<ChooseDateForm>()
                    .Field(new FieldReflector<ChooseDateForm>(nameof(Date))
                            .SetType(null)
                            .SetDefine((state, field) =>
                            {
                                foreach (var prod in GetDates())
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
                                    foreach (var movieName in GetDates())
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

        public static IFormDialog<ChooseDateForm> BuildDatesDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildDatesForm, options);
        }

        static List<string> GetDates()
        {
            return datesList;
        }
    }
}
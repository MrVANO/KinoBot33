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
    public class ChooseMovieFormatForm
    {
        public static List<string> formatList = new List<string>();

        [Prompt("Форматы фильма {||}")]
        [Template(TemplateUsage.NotUnderstood, "Вы ввели значение \"{0}\", которого нет на последней форме. Выберите значение из последней формы.")]
        public string Format { get; set; }

        public static IForm<ChooseMovieFormatForm> BuildDatesForm()
        {

            return new FormBuilder<ChooseMovieFormatForm>()
                    .Field(new FieldReflector<ChooseMovieFormatForm>(nameof(Format))
                            .SetType(null)
                            .SetDefine((state, field) =>
                            {
                                foreach (var prod in GetFormats())
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
                                    foreach (var movieName in GetFormats())
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

        public static IFormDialog<ChooseMovieFormatForm> BuildDatesDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildDatesForm, options);
        }

        static List<string> GetFormats()
        {
            return formatList;
        }
    }
}
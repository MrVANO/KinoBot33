using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinoBot2.Forms
{
    public enum Options {IGNORE, ФильмыНаНеделе};

    [Serializable]
    public class ChooseOptionForm
    {
        public static string message="";
        [Template(TemplateUsage.EnumSelectOne, "Выберите действие {||}")]
        [Template(TemplateUsage.NotUnderstood, "Вы ввели значение \"{0}\", которого нет на последней форме. Выберите значение из последней формы.")]
        public Options actionOptions { get; set; }

        public static IForm<ChooseOptionForm> BuildOptionsForm()
        {
            IFormBuilder<ChooseOptionForm> builder = new FormBuilder<ChooseOptionForm>()
                    //.Message(message)
                    .Field(nameof(actionOptions),
                    validate: async (state, response) =>
                    {
                        var result = new ValidateResult { IsValid = false, Value = response };
                        var movie = (response as string);
                        if(Options.ФильмыНаНеделе.ToString().Equals(result.Value.ToString()))
                        {
                            result.Feedback = "Запрос корректен";
                            result.IsValid = true;
                        }
                        if (!result.IsValid)
                        {
                            result.Feedback = "Случилась непредвиденная ошибка!!! Производится выход";
                            throw new OperationCanceledException();
                        }
                        return result;
                    }
                    ).AddRemainingFields();

            return builder.Build();
        }

        public static IFormDialog<ChooseOptionForm> BuildOptionsDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildOptionsForm, options);
        }
    }
}
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

                                return Task.FromResult(true);
                            }))
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
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

                                return Task.FromResult(true);
                            }))
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
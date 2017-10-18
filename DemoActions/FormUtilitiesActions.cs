using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using CommonClassUtils;
using OpenQA.Selenium.Support.PageObjects;

namespace DemoActions
{
    public class FormUtilitiesActions
    {

        public static void EnterTextIntoInputBox(string elementId, string valueToSend)
        {
            if (valueToSend == "") return;
            IWebElement inputBoxElement = Driver.Instance.FindElement(By.Id(elementId));
            inputBoxElement.Clear();
            inputBoxElement.SendKeys(valueToSend);
        }

        public static void SelectItemFromDropdown(string elementId, string valueToSelect)
        {
            if (valueToSelect == "") return;
            SelectElement dropdownElement = new SelectElement(Driver.Instance.FindElement(By.Id(elementId)));
            dropdownElement.SelectByText(valueToSelect);
        }

        public static void ChooseRadioOption(string elementId, string optionToSelect)
        {

            // Find the checkbox or radio button element by Name
            IList<IWebElement> oCheckBox = Driver.Instance.FindElements(By.Name(elementId));

            // This will tell you the number of checkboxes are present
            int Size = oCheckBox.Count;

            // Start the loop from first checkbox to last checkboxe
            for (int i = 0; i < Size; i++)
            {
                // Store the checkbox name to the string variable, using 'Value' attribute
                String Value = oCheckBox.ElementAt(i).GetAttribute("value");

                // Select the checkbox it the value of the checkbox is same what you are looking for
                if (Value.Equals(optionToSelect))
                {
                    oCheckBox.ElementAt(i).Click();
                    // This will take the execution out of for loop
                    break;
                }
            }
        }

        public static void ClickButton(string elementId)
        {
            IWebElement buttonElement = Driver.Instance.FindElement(By.Id(elementId));
            buttonElement.Click();
        }
    }



}

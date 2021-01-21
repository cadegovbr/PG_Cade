using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.UI.Mvc.Tests
{
    public class Util
    {
       // private static string DRIVER_PATH = @"C:\SeleniumDriver";
       //private static string ScreenShotLocation = @"C:\ScreenShot";
        private static string usuario_adfs = "t4.fecalado.pgd@cgu.gov.br";
        private static string senha_adfs = "fabricapgd04";
        private static string usuario_adfs_paulo = "t3.thiagogb.pgd@cgu.gov.br";
        private static string senha_adfs_paulo = "vixpgd3";
        public static IWebDriver GetDriverAndNavigate()
        {
            //var options = new InternetExplorerOptions();
            //options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            //options.RequireWindowFocus = true;
            //options.InitialBrowserUrl = "https://localhost:44336/";
            //var driver = new InternetExplorerDriver(DRIVER_PATH,options);
            var driver = new ChromeDriver(@"D:/Chrome");
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //var alert = wait.Until(ExpectedConditions.AlertIsPresent());
            //alert.SetAuthenticationCredentials("paulo.silva@gsw.com.br", "P@ulo11!");
            //driver.Navigate().GoToUrl("https://localhost:44338/");
            //Task.Delay(3000).Wait();
            driver.Navigate().GoToUrl("https://pgd-h.cgu.gov.br");
            IWebElement emailInicial = driver.FindElement(By.Id("emailInput"));
            emailInicial.SendKeys("t4.fecalado.pgd@cgu.gov.br");
            IWebElement submit = driver.FindElement(By.Name("HomeRealmByEmail"));
            submit.Click();
            IWebElement senhaInicial = driver.FindElement(By.Id("passwordInput"));
            senhaInicial.SendKeys("fabricapgd04");
            IWebElement submitEntrar = driver.FindElement(By.Id("submitButton"));
            submitEntrar.Click();
            //IWebElement submitEntrarLabs = driver.FindElement(By.CssSelector("[tabindex='1']"));
            ///submitEntrarLabs.Click();
            
            //LogarADFS(driver);
            SelecionaPerfil(driver);
           

            return driver;
        }
        public static IWebDriver GetDriverAndNavigatePacto()
        {
            var driver = new ChromeDriver(@"D:/Chrome");
            driver.Navigate().GoToUrl("https://pgd-h.cgu.gov.br");
            IWebElement emailInicial = driver.FindElement(By.Id("emailInput"));
            emailInicial.SendKeys("t1.fecalado.pgd@cgu.gov.br");
            IWebElement submit = driver.FindElement(By.Name("HomeRealmByEmail"));
            submit.Click();
            IWebElement senhaInicial = driver.FindElement(By.Id("passwordInput"));
            senhaInicial.SendKeys("fabricapgd01");
            IWebElement submitEntrar = driver.FindElement(By.Id("submitButton"));
            submitEntrar.Click();
            SelecionaPerfil(driver);
            return driver;
        }
        public static void WaitForTitle(IWebDriver driver, string title)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.Title.ToLower().Contains(title.ToLower()); });
        }

        public static IWebElement WaitForElement(IWebDriver driver, string id)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            return wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
        }
        internal static void Iniciar(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://localhost:8888/pgdlocal");
        }
        internal static void SelecionaPerfil(IWebDriver driver,bool solicitante = false)
        {
            if (driver.Title.Contains("Selecionar Perfil"))
            {
                IWebElement perfil = driver.FindElement(solicitante?By.ClassName("cssSolicitante"): By.Id("Perfil"));
                perfil.Click();

                IWebElement submit = driver.FindElement(By.Id("btnAutenticar"));
                submit.Click();
            }
        }
        internal static void LogarADFS(IWebDriver driver,bool paulo = false)
        {
            IWebElement username = driver.FindElement(By.Id("userNameInput"));
            IWebElement password = driver.FindElement(By.Id("passwordInput"));
            username.SendKeys(paulo?usuario_adfs_paulo: usuario_adfs);
            password.SendKeys(paulo?senha_adfs_paulo: senha_adfs);
            Util.TakeScreenShot(driver, "ADFS_");
            IWebElement submit = driver.FindElement(By.Id("submitButton"));
            submit.Click();
        }

        internal static void signOut(IWebDriver driver)
        {
            if (IsElementPresent(driver, By.Id("linkSignOut")))
            {
                var signOut = driver.FindElement(By.Id("linkSignOut"));
                signOut.Click();
            }
            IAlert alert = null;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { alert = d.SwitchTo().Alert(); return alert; });
            alert.Accept();
        }

        private static bool IsElementPresent(IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static void TakeScreenShot(IWebDriver driver, string filename)
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            //ss.SaveAsFile(string.Format("{0}\\{1}_{2}.jpg", ScreenShotLocation, TestContext.CurrentContext.Test.Name, filename), System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static void PerformMenu(IWebDriver driver, string menu, string submenu)
        {
            var element = WaitForElement(driver, menu);

            Actions action = new Actions(driver);

            action.MoveToElement(element).Perform();

            if (!IsElementPresent(driver, By.Id(submenu)))
                action.MoveToElement(element).Perform();

            element = WaitForElement(driver, submenu);
            Task.Delay(2000).Wait();
            driver.FindElement(By.Id(submenu)).Click();

        }

        public static string ToStringNullSafe(object value)
        {
            return (value ?? string.Empty).ToString();
        }

        /// <summary>
        ///  Método de Assert para selenium no insert e Edit do sistema.
        /// </summary>
        /// <param name="antesString">String para screenshot do Selenium</param>
        /// <param name="erroString">String para screenshot de erro</param>
        /// <param name="driver">Driver Selenium</param>
        /// <param name="resultado">Resultado esperado</param>
        public static void InsertEditAssert(string antesString, string erroString, IWebDriver driver, bool resultado)
        {
            if (resultado)
            {
                var element = Util.WaitForElement(driver, "divAlerta");
                IWebElement successMessage = driver.FindElement(By.Id("divAlerta"));

                Util.TakeScreenShot(driver, antesString);
                Assert.IsTrue(successMessage.Text.Contains("sucesso"));
            }
            else
            {

                var element = Util.WaitForElement(driver, "validationMessage");
                IWebElement successMessage = driver.FindElement(By.Id("validationMessage"));

                Util.TakeScreenShot(driver, erroString);
                Assert.IsTrue(successMessage.Text.Contains("preenchimento obrigatório"));
            }
        }
        /// <summary>
        /// Seleciona um item do Listbox pelo Index
        /// </summary>
        /// <param name="id">Identificador do Listbox</param>
        /// <param name="index">Dispensa comentários</param>
        /// <param name="driver">Driver Selenium</param>
        public static void SelecionaListBoxByIndex(string id, int index, IWebDriver driver)
        {
            //var listAtividades = driver.FindElement(By.Id(id));
            //var select = new SelectElement(driver.FindElement(By.Id(id)));
            //select.SelectByIndex(index);

            var selectableItems = driver.FindElements(By.XPath("//li[contains(@id,'selectable')]/*"));

            //Build the select Item action.
            Actions toSelect = new Actions(driver);
            toSelect.Click(selectableItems[index]);

            //Perform action.
            var selectItems = toSelect.Build();
            selectItems.Perform();
        }

        public static void SelecionaEditar(int index, IWebDriver driver)
        {
            var listaItens = driver.FindElements(By.XPath("//a[contains(@href,'Create/')]"));
            Actions toSelect = new Actions(driver);
            toSelect.Click(listaItens[index]);

            //Perform action.
            var selectItems = toSelect.Build();
            selectItems.Perform();
        }

        public static void SelecionaDeletar(int index, IWebDriver driver)
        {
            var listaItens = driver.FindElements(By.XPath("//a[contains(@onclick,'return')]"));
            Actions toSelect = new Actions(driver);
            toSelect.Click(listaItens[index]);

            //Perform action.
            var selectItems = toSelect.Build();
            selectItems.Perform();
        }

    }
}

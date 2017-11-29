package ua.feo;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.openqa.selenium.*;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.support.ui.Select;

import java.util.Set;
import java.util.concurrent.TimeUnit;

import static org.junit.Assert.*;
import static ua.feo.Data.*;

public class ChromeTest {

    private WebDriver driver;
    private String baseUrl;
    private StringBuffer verificationErrors = new StringBuffer();

    @Before
    public void setUp() throws Exception {

        driver = new ChromeDriver();
        baseUrl = "https://prometheus.org.ua/";
        driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
    }

    @After
    public void tearDown() throws Exception {
        driver.quit();
        String verificationErrorString = verificationErrors.toString();
        if (!"".equals(verificationErrorString)) {
            fail(verificationErrorString);
        }
    }

    @Test
    public void testLogin() throws Exception {
        driver.get(baseUrl);
        driver.findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click();
        driver.findElement(By.id("login-email")).sendKeys(USER_EMAIL);
        driver.findElement(By.id("login-password")).sendKeys(USER_PASSWORD);
        driver.findElement(By.xpath("//button[@type='submit']")).click();
        assertEquals(USER_DISPLAYNAME, driver.findElement(By.xpath("//header[@id='global-navigation']/nav/ol[2]/li/a/div")).getText());
    }

    @Test
    public void testLoginFail() throws Exception {
        driver.get(baseUrl);
        driver.findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click();
        driver.findElement(By.id("login-email")).sendKeys(USER_EMAIL);
        driver.findElement(By.id("login-password")).sendKeys(USER_EMAIL);
        driver.findElement(By.xpath("//button[@type='submit']")).click();
        assertEquals("Електронна адреса або пароль невірні.", driver.findElement(By.xpath("//div[@id='login-form']/div[2]/div[2]/ul/li")).getText());
        assertEquals("Неможливо увійти", driver.findElement(By.xpath("//div[@id='login-form']/div[2]/div[2]/h4")).getText());
    }

    @Test
    public void testRegisterFail() throws Exception {
        driver.get(baseUrl);
        driver.findElement(By.xpath("(//a[contains(text(),'Зареєструватися')])[2]")).click();
        driver.findElement(By.id("register-email")).sendKeys(USER_EMAIL);
        driver.findElement(By.id("register-username")).sendKeys(USER_DISPLAYNAME);
        driver.findElement(By.id("register-name")).sendKeys(USER_NAME);
        driver.findElement(By.id("register-password")).sendKeys(USER_PASSWORD);
        new Select(driver.findElement(By.id("register-gender"))).selectByVisibleText(USER_GENDER);
        new Select(driver.findElement(By.id("register-year_of_birth"))).selectByVisibleText(USER_YEAR);
        new Select(driver.findElement(By.id("register-level_of_education"))).selectByVisibleText(USER_STUDY);
        driver.findElement(By.id("register-mailing_address")).sendKeys(USER_CITY);
        driver.findElement(By.id("register-honor_code")).click();
        driver.findElement(By.xpath("//button[@type='submit']")).click();
        assertEquals("Здається " + USER_DISPLAYNAME + " вже використовується. Спробуйте ще раз з іншим іменем користувача.", driver.findElement(By.xpath("//div[@id='register-form']/div/ul/li")).getText());
        assertEquals("Ми не змогли створити Ваш обліковий запис", driver.findElement(By.xpath("//div[@id='register-form']/div/h4")).getText());
        assertEquals("Здається " + USER_EMAIL + " вже використовується. Спробуйте ще раз з іншою адресою.", driver.findElement(By.xpath("//div[@id='register-form']/div/ul/li[2]")).getText());
    }

    @Test
    public void testLinkChecker() throws Exception {
        driver.get(baseUrl);
        driver.findElement(By.xpath("//img[contains(@src,'https://prometheus.org.ua/wp-content/uploads/2014/09/logo_white_wp.png')]")).click();
        assertEquals("", driver.findElement(By.xpath("//img[contains(@src,'https://prometheus.org.ua/wp-content/uploads/2014/09/logo_white_wp.png')]")).getText());
    }

    @Test
    public void testCanRegisterCourse() throws Exception {
        driver.get(baseUrl);
        driver.findElement(By.xpath("(//a[contains(text(),'Курси')])[2]")).click();
        ((JavascriptExecutor) driver).executeScript("window.scrollBy(0,2500)", "");
        driver.findElement(By.linkText(COURSE_TITLE_1)).click();
        assertEquals("ЗАРЕЄСТРУВАТИСЬ НА КУРС", driver.findElement(By.className("register")).getText());
    }

    @Test
    public void testFirstVideoLength() throws Exception {
        driver.get(baseUrl);
        driver.findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click();
        driver.findElement(By.id("login-email")).sendKeys(USER_EMAIL);
        driver.findElement(By.id("login-password")).sendKeys(USER_PASSWORD);
        driver.findElement(By.xpath("//button[@type='submit']")).click();
        driver.findElement(By.linkText(COURSE_TITLE_2)).click();
        driver.findElement(By.linkText("Курс")).click();
        assertEquals(COURSE_TIME_2, driver.findElement(By.xpath("//div[@id='video_i4x-IRF-ML101-video-08cff3a27e65461cb193cc2e6f985a41']/div[2]/div/div[5]/div[2]/div/div")).getText().split(" ")[2]);
    }

    @Test
    public void testStartVideo() throws Exception {
        driver.get(baseUrl);
        driver.findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click();
        driver.findElement(By.id("login-email")).sendKeys(USER_EMAIL);
        driver.findElement(By.id("login-password")).sendKeys(USER_PASSWORD);
        driver.findElement(By.xpath("//button[@type='submit']")).click();
        driver.findElement(By.linkText("Машинне навчання")).click();
        driver.findElement(By.linkText("Курс")).click();
        driver.findElement(By.xpath("//div[@id='video_i4x-IRF-ML101-video-08cff3a27e65461cb193cc2e6f985a41']/div[2]/div/div[5]/div[2]/div/button/span")).click();
        Thread.sleep(3000);
        assertNotEquals(COURSE_START_TIME, driver.findElement(By.xpath("//div[@id='video_i4x-IRF-ML101-video-08cff3a27e65461cb193cc2e6f985a41']/div[2]/div/div[5]/div[2]/div/div")).getText().split(" ")[0]);
    }

    @Test
    public void testProfileImage() throws Exception {
        driver.get(baseUrl);
        driver.findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click();
        driver.findElement(By.id("login-email")).sendKeys(USER_EMAIL);
        driver.findElement(By.id("login-password")).sendKeys(USER_PASSWORD);
        driver.findElement(By.xpath("//button[@type='submit']")).click();
        driver.findElement(By.xpath("//header[@id='global-navigation']/nav/ol[2]/li[2]/button/span[2]")).click();
        driver.findElement(By.linkText("Профіль")).click();
        assertEquals(USER_DISPLAYNAME, driver.findElement(By.id("u-field-value-username")).getText());
        driver.get(USER_PROFILE_IMAGE);
        Thread.sleep(2000);
    }

    @Test
    public void testCookies() throws Exception {
        driver.get(baseUrl);
        Set<Cookie> cookies = driver.manage().getCookies();
        cookies.forEach(c -> System.out.println(c.getName()));
        assertNotEquals(0, cookies.size());
    }

    @Test
    public void testDeleteCookies() throws Exception {
        driver.get(baseUrl);
        driver.manage().deleteAllCookies();
        Set<Cookie> cookies = driver.manage().getCookies();
        assertEquals(0, cookies.size());
    }

    @Test
    public void testGoogleSearch() throws Exception {
        driver.get("https://www.google.com.ua/");
        driver.findElement(By.id("lst-ib")).sendKeys("prometheus");
        driver.findElement(By.id("lst-ib")).sendKeys(Keys.ENTER);
        assertEquals("Prometheus – масові безкоштовні онлайн-курси", driver.findElement(By.linkText("Prometheus – масові безкоштовні онлайн-курси")).getText());
    }

}

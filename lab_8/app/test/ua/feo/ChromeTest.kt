package ua.feo

import org.junit.After
import org.junit.Before
import org.junit.Test
import org.openqa.selenium.By
import org.openqa.selenium.JavascriptExecutor
import org.openqa.selenium.Keys
import org.openqa.selenium.WebDriver
import org.openqa.selenium.chrome.ChromeDriver
import org.openqa.selenium.support.ui.Select
import ua.feo.data.*
import java.util.concurrent.TimeUnit
import kotlin.test.*

class ChromeTest {
    
    private var driver: WebDriver = NullWebDriver
    private var baseUrl: String = ""
    private val verificationErrors = StringBuffer()

    @Throws(Exception::class)
    @Before fun setUp() {
        driver = ChromeDriver()
        baseUrl = "https://prometheus.org.ua/"
        driver.manage().timeouts().implicitlyWait(60, TimeUnit.SECONDS)
    }

    @Throws(Exception::class)
    @After fun tearDown() = with(driver) {
        quit()
        val verificationErrorString = verificationErrors.toString()
        if ("" != verificationErrorString) {
            fail(verificationErrorString)
        }
    }

    @Throws(Exception::class)
    @Test fun testLogin() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click()
        findElement(By.id("login-email")).sendKeys(USER_EMAIL)
        findElement(By.id("login-password")).sendKeys(USER_PASSWORD)
        findElement(By.xpath("//button[@type='submit']")).click()
        assertEquals(USER_DISPLAYNAME, findElement(By.xpath("//header[@id='global-navigation']/nav/ol[2]/li/a/div")).text)
    }

    @Throws(Exception::class)
    @Test fun testLoginFail() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click()
        findElement(By.id("login-email")).sendKeys(USER_EMAIL)
        findElement(By.id("login-password")).sendKeys(USER_EMAIL)
        findElement(By.xpath("//button[@type='submit']")).click()
        assertEquals("Електронна адреса або пароль невірні.", findElement(By.xpath("//div[@id='login-form']/div[2]/div[2]/ul/li")).text)
        assertEquals("Неможливо увійти", findElement(By.xpath("//div[@id='login-form']/div[2]/div[2]/h4")).text)
    }

    @Throws(Exception::class)
    @Test fun testRegisterFail() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("(//a[contains(text(),'Зареєструватися')])[2]")).click()
        findElement(By.id("register-email")).sendKeys(USER_EMAIL)
        findElement(By.id("register-username")).sendKeys(USER_DISPLAYNAME)
        findElement(By.id("register-name")).sendKeys(USER_NAME)
        findElement(By.id("register-password")).sendKeys(USER_PASSWORD)
        Select(findElement(By.id("register-gender"))).selectByVisibleText(USER_GENDER)
        Select(findElement(By.id("register-year_of_birth"))).selectByVisibleText(USER_YEAR)
        Select(findElement(By.id("register-level_of_education"))).selectByVisibleText(USER_STUDY)
        findElement(By.id("register-mailing_address")).sendKeys(USER_CITY)
        findElement(By.id("register-honor_code")).click()
        findElement(By.xpath("//button[@type='submit']")).click()
        assertEquals("Здається $USER_DISPLAYNAME вже використовується. Спробуйте ще раз з іншим іменем користувача.", findElement(By.xpath("//div[@id='register-form']/div/ul/li")).text)
        assertEquals("Ми не змогли створити Ваш обліковий запис", findElement(By.xpath("//div[@id='register-form']/div/h4")).text)
        assertEquals("Здається $USER_EMAIL вже використовується. Спробуйте ще раз з іншою адресою.", findElement(By.xpath("//div[@id='register-form']/div/ul/li[2]")).text)
    }

    @Throws(Exception::class)
    @Test fun testLinkChecker() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("//img[contains(@src,'https://prometheus.org.ua/wp-content/uploads/2014/09/logo_white_wp.png')]")).click()
        assertEquals("", findElement(By.xpath("//img[contains(@src,'https://prometheus.org.ua/wp-content/uploads/2014/09/logo_white_wp.png')]")).text)
    }

    @Throws(Exception::class)
    @Test fun testCanRegisterCourse() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("(//a[contains(text(),'Курси')])[2]")).click()
        (this as JavascriptExecutor).executeScript("window.scrollBy(0,2500)", "")
        findElement(By.linkText(COURSE_TITLE_1)).click()
        assertEquals("ЗАРЕЄСТРУВАТИСЬ НА КУРС", findElement(By.className("register")).text)
    }

    @Throws(Exception::class)
    @Test fun testFirstVideoLength() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click()
        findElement(By.id("login-email")).sendKeys(USER_EMAIL)
        findElement(By.id("login-password")).sendKeys(USER_PASSWORD)
        findElement(By.xpath("//button[@type='submit']")).click()
        findElement(By.linkText(COURSE_TITLE_2)).click()
        findElement(By.linkText("Курс")).click()
        assertEquals(COURSE_TIME_2, findElement(By.xpath("//div[@id='video_i4x-IRF-ML101-video-08cff3a27e65461cb193cc2e6f985a41']/div[2]/div/div[5]/div[2]/div/div")).text.split(" ".toRegex()).dropLastWhile { it.isEmpty() }.toTypedArray()[2])
    }

    @Throws(Exception::class)
    @Test fun testStartVideo() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click()
        findElement(By.id("login-email")).sendKeys(USER_EMAIL)
        findElement(By.id("login-password")).sendKeys(USER_PASSWORD)
        findElement(By.xpath("//button[@type='submit']")).click()
        findElement(By.linkText("Машинне навчання")).click()
        findElement(By.linkText("Курс")).click()
        findElement(By.xpath("//div[@id='video_i4x-IRF-ML101-video-08cff3a27e65461cb193cc2e6f985a41']/div[2]/div/div[5]/div[2]/div/button/span")).click()
        Thread.sleep(3000)
        assertNotEquals(COURSE_START_TIME, findElement(By.xpath("//div[@id='video_i4x-IRF-ML101-video-08cff3a27e65461cb193cc2e6f985a41']/div[2]/div/div[5]/div[2]/div/div")).text.split(" ".toRegex()).dropLastWhile { it.isEmpty() }.toTypedArray()[0])
    }

    @Throws(Exception::class)
    @Test fun testProfileImage() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("(//a[contains(text(),'Увійти')])[2]")).click()
        findElement(By.id("login-email")).sendKeys(USER_EMAIL)
        findElement(By.id("login-password")).sendKeys(USER_PASSWORD)
        findElement(By.xpath("//button[@type='submit']")).click()
        findElement(By.xpath("//header[@id='global-navigation']/nav/ol[2]/li[2]/button/span[2]")).click()
        findElement(By.linkText("Профіль")).click()
        assertEquals(USER_DISPLAYNAME, findElement(By.id("u-field-value-username")).text)
        get(USER_PROFILE_IMAGE)
        Thread.sleep(2000)
    }

    @Throws(Exception::class)
    @Test fun testCookies() = with (driver) {
        get(baseUrl)
        val cookies = manage().cookies
        cookies.forEach { c -> println(c.name) }
        assertNotEquals(0, cookies.size)
    }

    @Throws(Exception::class)
    @Test fun testDeleteCookies() = with(driver) {
        get(baseUrl)
        manage().deleteAllCookies()
        val cookies = manage().cookies
        assertEquals(0, cookies.size)
    }

    @Throws(Exception::class)
    @Test fun testGoogleSearch() = with(driver) {
        get("https://www.google.com.ua/")
        findElement(By.id("lst-ib")).sendKeys("prometheus")
        findElement(By.id("lst-ib")).sendKeys(Keys.ENTER)
        assertEquals("Prometheus – масові безкоштовні онлайн-курси", findElement(By.linkText("Prometheus – масові безкоштовні онлайн-курси")).text)
    }

    @Throws(Exception::class)
    @Test fun testCountWords() = with(driver) {
        get("https://www.google.com.ua/")
        findElement(By.id("lst-ib")).sendKeys("testcase")
        findElement(By.id("lst-ib")).sendKeys(Keys.ENTER)
        findElement(By.linkText("Про Тестинг - Тестирование - Тестовый случай - Test Case")).click()
        val text = findElement(By.tagName("body")).text.toLowerCase()
        println("testcase : ${text.split("testcase").size - 1}")
        println("test : ${text.split("test").size - 1}")
        println("case : ${text.split("case").size - 1}")
    }

    @Throws(Exception::class)
    @Test fun testSearch() = with(driver) {
        get(baseUrl)
        findElement(By.xpath("(//a[contains(text(),'Блог')])[2]")).click()
        Thread.sleep(10000)
        findElement(By.id("s")).sendKeys("kotlin")
        findElement(By.id("searchsubmit")).click()
        Thread.sleep(3000)
        assertEquals("Not Found", driver.findElement(By.xpath("//div/div/div/div/div/div/div/div[2]")).text)
    }

    @Throws(Exception::class)
    @Test fun testNew() = with(driver) {
        get("http://vns.lpnu.ua/")
        findElement(By.linkText("Log in")).click()
        findElement(By.id("username")).sendKeys("Bohdan.Duben.PI.2015")
        findElement(By.id("password")).sendKeys("07.01.1995")
        findElement(By.id("loginbtn")).click()
        findElement(By.id("action-menu-toggle-0")).click()
        findElement(By.id("actionmenuaction-2")).click()
        findElement(By.linkText("Редагувати інформацію")).click()
        (0..0).forEach {
            findElement(By.xpath("//button[@class='atto_image_button']")).click()
            findElement(By.id("id_description_editor_atto_image_urlentry")).sendKeys("https://i.pinimg.com/736x/1d/dd/97/1ddd97b91e6f1a431c9c73fee8b79a70--pinky-pie-pony-party.jpg")
            findElement(By.id("id_description_editor_atto_image_altentry")).sendKeys("yee")
            findElement(By.xpath("//button[@class='atto_image_urlentrysubmit']")).click()
        }
    }

}
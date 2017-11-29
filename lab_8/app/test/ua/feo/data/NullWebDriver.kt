package ua.feo.data

import org.openqa.selenium.By
import org.openqa.selenium.WebDriver
import org.openqa.selenium.WebElement

object NullWebDriver : WebDriver {
    override fun getWindowHandles(): MutableSet<String> = mutableSetOf()
    override fun findElement(p0: By?): WebElement = findElement(p0)
    override fun getWindowHandle(): String = ""
    override fun getPageSource(): String = ""
    override fun navigate(): WebDriver.Navigation = navigate()
    override fun manage(): WebDriver.Options = manage()
    override fun getCurrentUrl(): String = ""
    override fun getTitle(): String = ""
    override fun get(p0: String?) = Unit
    override fun switchTo(): WebDriver.TargetLocator = switchTo()
    override fun close() = Unit
    override fun quit() = Unit
    override fun findElements(p0: By?): MutableList<WebElement> = mutableListOf()
}
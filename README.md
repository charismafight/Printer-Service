# Printer-Service

PrinterService, an application for OS to implement silent print.
WebPrinter, a website for user to upload file and send print request to printers without wifi.


# Updated 2024-11-28

Trying to use .net core web to receive files to print is a dump choice, as IIS restriction always prevents application from executing powershell scripts, and even without any prompt and log. So I want to change the project into windows service with minimal http API.

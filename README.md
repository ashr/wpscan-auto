# wpscan-auto
Automated wpscans and email delivery of reports (For scanning multiple wordpress sites on a schedule, like through crontab)

Add the urls of the wordpress sites you want scanned as well as the emails that will receive the report to wordpress-urls-and-users.txt

Change the from address, SMTP server in app.config.

In Debug mode this app will not send emails, it will just print the email addresses the report would be sent to.

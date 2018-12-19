# SMB Demo .NET Framework

This repository is a trivial implementation of a .NET framework web application utilized to show features related to connecting a cloud foundry application to a remote SMB Share.  This application will allow you to perform simple CRUD operations on a the SMB share directory and also review the permissions set on that directory.  The credentials to connect to the share are exposed by an user provided service.

1. Clone the application to your local working environment with the following command: `git clone https://github.com/tezizzm/SmbDemoFramework`

2. From the location you cloned the git repository, navigate to the project directory using the following command: `cd .\src\LighthouseWeb\`

3. If you have not already previously created an SMB Share see the following links which give instructions for creating an SMB Share in [Powershell](https://docs.microsoft.com/en-us/powershell/module/smbshare/new-smbshare?view=win10-ps) or using [Windows Explorer or Computer Management](https://docs.microsoft.com/en-us/windows-hardware/drivers/debugger/file-share--smb--symbol-server)

4. Open the file `.profile.bat` in your editor of choice.  Take of the place holders denoted with `<>`, this is here where you will enter information about your Windows hosted SMB Share.

    *NOTE: If you have special characters in your password it may be necessary to surround your entry in double quotes see the example implementation.*

    ```batch
        net use \\<share ip address>\<share name> /user:<username> <password>
    ```

    ```batch
        net use \\123.456.789.012\share /user:admin "password123!@#@$,"
    ```

5. Once done entering your SMB share information save and close the `.profile.bat` file

6. We will now create a user provided service which our application will use to communicate with SMB share.

7. Open the `service-config.json` file and note it's contents.  Once again in the placeholders, you will need to enter the credentials of your SMB Share.

    *Note the values in your `service-config.json` file should match those in your `.profile.bat` file*

    ```json
    {
        "url": "\\\\<ip address>\\<share name>",
        "username": "<username>",
        "password": "<password>"
    }
    ```

    ```json
    {
        "url": "\\\\123.456.789.012\\share",
        "username": "admin",
        "password": "password123!@#@$,"
    }
    ```
8. Once done entering your SMB share information save and close the `service-config.json` file

9. Now that the configuration is set, we will create this service in cloud foundry with the following command: `cf create-user-provided-service winfs-framework -p .\service-config.json`

10. Take note of the `manifest.yml` file.  We are binding our application to the user provided service we created.

    ```yml
    ---
    applications:
    - name: lighthouseweb
    buildpack: hwc_buildpack
    health-check-type: http
    stack: windows2016
    services:
    - winfs-framework
    ```
11. We will now deploy our application to Cloud Foundry by running the following command `cf push`.

    *Note: It can take a few moments for your application to successfully make the connection to the external SMB server.  It may be necessary to push your app again to ensure that the connection is made prior to health check timeout.  If the error persists beyond several tries please inspect the logs as something other error could be preventing a successful start.*

12. Click around the app.  If you have files in the directory they will be read from the SMB share and listed on the initial (Index) page.  You also have the opportunity to create a file on the SMB share by uploading a file from your local system.  Once created the file should show up in the index.  You can also read a file from the share and see processing of that files contents (approximate line numbers, word counts and content).

13. In the permissions tab you will see the user currently used to access the share as well as the users and that are assigned permissions on the share.  Note you will see several common Windows account (NT AUTHORITY\SYSTEM, BUILTIN\Administrators) as well as some accounts listed with their SID (S-1-5-21-24743843688-3923987316-3868828779-1000).  

14. Play around with the permissions on the shared folder and/or create multiple shares with different permissions.  

15. Use the command `cf update-service winfs-framework -p .\service-config.json` to update your service.  Note you will have to make edits to the `service-config.json` that correspond to your SMB share changes.

16. As you update the user provided service to toggle the share you're connected to or the user you're connecting as, note how in some cases you will not be able to read from the directory or you will not be able to upload a file to the share.
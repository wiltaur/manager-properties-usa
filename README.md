# manager-properties-usa
Information about properties in the United States.

This is a Restful API developed for testing at Million And Up.

The DB that is used is local with Sql Server "**SQLEXPRESS**" and we worked with the user sa, giving it an initial password with which it connected to the ORM Entity Framework. The Database scripts are in the folder: **"scripts_db"**.

To keep in mind:
- For security, to use the developed microservices a token must be generated first (The AuthenticateUser method is used for these), then passed through the header using authentication bearer.
- This development is using the properties of the environment variables **(Properties/launchSettings.json)**, so that in a future deployment a YML file is created and in it configure the secrets that are created in the cloud, for example that file should contain at least the db connection string:
~~~
    ...
    envFrom:
        - secretRef:
           name: main-db
~~~
  ... With this, for security the **"Properties/launchSettings.json"** file should no longer contain such environment variables.
- The images must reach the API in BASE64 format, likewise in the query it is returned in the same format. In the Database it saves an Array of bytes. For functional tests you can use the WebSite "https://www.base64-image.de/" and upload an image to generate the Base64 format, note that the value to use must not contain the initial part "data:image/jpeg;base64,".
- The method that lists the properties was developed thinking of a view with a table with a search field, the indicated columns with the option of ordering click and also with a pagination.
- Some unit tests were developed with NUnit to the Controller and the logic data.

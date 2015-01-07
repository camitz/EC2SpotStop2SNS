Service/App to detect spot instances terminations and publish to SNS for further customized notification for example sending emails or text.

AWS notifies spot instances of imminent termination, for example due to the spot price exceding the requested max price, by means of [instance meta data](). When termination has been scheduled, this time is made available though an http request to http://169.254.169.254/latest/meta-data/spot/termination-time. This application will poll the url every 5 minutes. If termination is detected it will post a message to SNS, Amazon's Simple Notification Service. An admin can configure SNS the pick up on this message and take the appropriate actions, for example send an email or a text. Other applications subscribing to the topic will also be notified of the event.

## Specifics

The installer will install the application as a windows service.

An SNS topic will automatically be created, if not existing. By default the topic is similar to *arn:aws:sns:eu-west-1:860264074053:SpotInstanceTermination*. Override this with

   <appSettings>
       <add key="Topic" value="MyTopic" />
   </appSettings>

Credentials and region is specified through any means available by AWS .net sdk, for example

   <appSettings>
       <add key="AWSRegion" value="eu-west-1" />
   </appSettings>

Windows Application/Service to detect spot instances terminations and publish to SNS for further customized notification for example sending emails or text.

##Summary

AWS notifies spot instances of imminent termination, for example due to the spot price exceding the requested max price, by means of [instance meta data](http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/ec2-instance-metadata.html). When termination has been scheduled, this time is made available though an http request to http://169.254.169.254/latest/meta-data/spot/termination-time. This application will poll the url every 5 seconds. If termination is detected it will post a message to SNS, Amazon's [Simple Notification Service](http://aws.amazon.com/sns/). An admin can configure SNS the pick up on this message and take the appropriate actions, for example send an email or a text. Other applications subscribing to the topic will also be notified of the event.

Read more about Spot instance termination notices [here](https://aws.amazon.com/blogs/aws/new-ec2-spot-instance-termination-notices/?sc_ichannel=em&sc_icountry=global&sc_icampaigntype=launch&sc_icampaign=em_130420040&sc_idetail=em_66267057&ref_=pe_395030_130420040_8).

## Specifics

An SNS topic will automatically be created, if not existing. By default the topic is similar to *arn:aws:sns:eu-west-1:860264074053:SpotInstanceTermination*. Override this with

   <appSettings>
       <add key="Topic" value="MyTopic" />
   </appSettings>

Credentials and region is specified through any means available by AWS .net sdk, for example

   <appSettings>
       <add key="AWSRegion" value="eu-west-1" />
   </appSettings>

If you'd like to debug this app locally, see the [MockECSInstanceMetaData](https://github.com/camitz/MockEC2InstanceMetaData) project.


##Windows binaries and installer

An installer can be downloaded [here](https://github.com/camitz/EC2SpotStop2SNS/releases/latest) which will install the application as a windows service.

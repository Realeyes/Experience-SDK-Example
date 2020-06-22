# Experience SDK example

Experience SDK is a collection of libraries for different platforms to collect and analyse human emotions data to measure user’s experience.

Experience SDK is responsible for:
- Handling UX/UI and communication for permissions (e.g. GDPR)
- Video stream receiving & pre-processing
- Emotion/Attention data extracting (using Realeyes Vision SDK)
- Capturing events for analysis
- Receiving/transporting data to/from Realeyes Data Platform
- Global time synchronization for events

Currently Realeyes have implemented and providing SDKs for web browser and mobile (Android) platforms. 
And the SDKs are providing all required tools for tracking expeirence data and communicating them to Realeyes Data Processing Platform. 

However there are plenty of different use-cases and platforms where Realeyes technologies can be used. 
And to cover these use-cases and platforms, we are providing our clients with guidences how to build their own Experience SDK with a set of low level modules, which are provided by Realeyes and used by already existing implementations of Expirience SDK.

In this guide we describe modules of Experience SDKs and provide an example of a custom SDK implemented in C# (.NET).

## Main modules 
Existing Experience SDKs have the following internal modules: 
- **Tracker** - public interface of Experience SDK
- **Permissions/Consent and UI modules** - obtaining user permissions (e.g. GDPR)
- **Camera/StreamSource** - video stream receiving & pre-processing
- **Vision SDK** - extracting Emotion/Attention data
- **Analytic Events** - capturing events for analysis
- **Data Transport** - receiving/transporting data to/from Realeyes Data Platform

To help with the process of implementating Experience SDK for a new platform we are providing a simplified example implementation of these modules in C#.

*NOTE*: it is not neccessary to follow the internal interfaces' specifications one to one, but important to allign with our [Data Ingestion API](https://developers.realeyesit.com/DL/)
and keep public tracker interface similar to existing Experience SDK.

In the example project you can find the following classes, which represent modules described above:
- **Tracker** - main & public interface of SDK
- **StreamSource** - Represents camera/stream source + Vision SDK modules. Implemented as a generator of random data in the example
- **EventProcessor** - Represents simplified Analytics event module
- **DataTransport** - Transporting data to Realeyes Data Platform. Aligns with our [Data Ingestion API](https://developers.realeyesit.com/DL/)
- **TimeSyncService** - Part of Analytics event module, helps keep events synced across multiple devices/app instances


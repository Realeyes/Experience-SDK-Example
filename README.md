# Experience SDK example

Experience SDK is a collection of libraries for different platforms to collect and analyse human emotions data to measure user’s experience.

Experience SDK is responsible for:
- Handling UX/UI and communication for permissions (GDPR)
- Video stream receiving & pre-processing
- Emotion/Attention data extracting (using Realeyes Vision SDK)
- Capturing events for analysis
- Receiving/transporting data to/from Realeyes Data Platform
- Global time synchronization for events

Currently Realeyes have implemented and providing SDKs for web browser and mobile(Android) platforms. 
And the SDKs are providing all required tools for tracking expeirence data and communicating them to REaleyes Data Processing Platform. 

However there are plenty different use-cases and platforms where Realeyes' technologies can be used. 
And to cover this use-cases and platforms, we are providing to our clients guidences, how to build their own Experience SDK with set of low level modules what is provided by REaleyes and used by their existing Expirience SDKs 

In this guide will be described modules of existing Experience SDKs, and provided an example of custom  SDK implemented with c#

## Main modules 
Existing Experience SDKs have following internal modules: 
- **Tracker** - public interface of Experience SDK
- **Permissions/Consent and UI modules** - obtaining user permissions (GDPR)
- **Camera/StreamSource** - video stream  receiving & pre-processing
- **Vision SDK** - extracting Emotion/Attention data
- **Analytic Events** - capture events for analysis
- **Data Transport** - Receiving/transporting data to/from Realeyes Data Platform

To simplify implementation of Custom experience SDK for our clients we have provided example of simplified modules' implementation.

*NOTE*: it is not neccessary to follow the internal interfaces' specifications 1:1, but important to allign with our [Data Ingestion API](https://developers.realeyesit.com/DL/)
and keep public tracker interface similar to existing Experience SDK.

In the example project you can find next classes which represents modules described above:
- **Tracker** - main & public interface of SDK
- **StreamSource** - Represents camera/streeam source  + Vision SDK modules. Implemented as generator of random data in the example
- **EventProcessor** - Represents simplified Analytics event module.
- **DataTransport** - Transporting data to Realeyes Data Platform. Aligns with our [Data Ingestion API](https://developers.realeyesit.com/DL/)
- **TimeSyncService** - Part of Analytics event module, helps to keep events synced across multiple devices/ app instances


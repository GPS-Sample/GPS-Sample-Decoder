<div align="center">
  <a href="https://www.gpssample.org">
    <img alt="GPSSample logo" src="images/gpssample.png" height="128">
  </a>
  <h1>GPSSampleDecoder</h1>
</div>

[![Platform](https://img.shields.io/badge/Platforms-Windows,Mac-3B90C4.svg?style=flat)](https://www.android.com)
[![Language](https://img.shields.io/badge/Language-CSharp-blue.svg?style=flat)](https://kotlinlang.org)
[![License](https://img.shields.io/badge/License-GPL%20v3-blue.svg?style=flat)](LICENSE.md)
[![Chat](https://img.shields.io/badge/Chat-on%20Discord-60C53A.svg?style=flat)](https://discord.com/channels/1369369021761327134/1369369022709370985)
[![Covenant](https://img.shields.io/badge/Contributor%20Covenant-1.19.24-469C9C.svg?style=flat)](CODE-OF-CONDUCT.pdf)

## About

**What is the GPSSample Decoder?**

The GPSSample Decoder is used to decrypt data from a study conducted in GPSSample using the study configuration with a password. The study password is set by the study Administrator in the GPSSample app and is case sensitive.  
Output from the GPSSample Decoder includes:

1)	An Excel workbook with all GPSSample data from enumerated, sampled, and surveyed households and points of interest.
2)	Field photos taken of households and of points of interest
3)	A decrypted configuration file (JSON) with all data from the GPSSample study

If multiple files from the same study are selected (ex. supervisor_day1, supervisor_day2, supervisor_day3), all will be decrypted and compiled into one Excel workbook and one JSON. The GPSSample Decoder allows the user to select where to save the outputs.

**Key GPSSample Decoder principles:**
•	The GPSSample Decoder does not require administrative privileges to use or run this application which would inhibit its usability.
•	The GPSSample Decoder includes a package of files required to run the code as a user.

Technical Specifications: Versions of the GPSSample Decoder are available for Windows and Mac computers. Check that the GPSSample Decoder is compatible with the version of the GPSSample app you are using.

**Technical Specifications:** Versions of the GPSSample Decoder are available for Windows and Mac computers. Check that the GPSSample Decoder is compatible with the version of the GPSSample app you are using.

## Training Guides

Visit [https://www.gpssample.org/resources/training-guides](https://www.gpssample.org/resources/training-guides) to view the training guides and to watch videos on using GPSSample.

## Development Environment

* Microsoft Windows PC
* Apple Mac Computer
* Visual Studio Community Edition 2022
  
## Contributing

There are many ways in which you can participate in this project, for example:

* [Submit bugs and feature requests](https://github.com/GPS-Sample/GPS-Sample-Decoder/issues), and help us verify as they are checked in
* Review [source code changes](https://github.com/GPS-Sample/GPS-Sample-Decoder/pulls)

If you are interested in fixing issues and contributing directly to the code base, please see the document [How to Contribute](How-to-Contribute.md).

## Build Instructions

* Install Visual Studio Community Edition 2022 on your Windows or Mac computer
* Clone the repo
* The main branch is main and is read-only on GitHub.  You can build/modify the main branch, but you will not be allowed to push your changes to main to GitHub. See [How to Contribute](How-to-Contribute.md) for instructions.
* Open the project in Visual Studio  
* Sync the project (requires internet) and build the solution
  
## Community

The GPSSample community can be found on [Discord](https://discord.com/channels/1369369021761327134/1369369022709370985) where you can ask questions, voice ideas, and share your projects with other people.

Do note that our [Code of Conduct](CODE-OF-CONDUCT.pdf) applies to all GPSSample community channels. Users are **highly encouraged** to read and adhere to them to avoid repercussions.

## Code of Conduct

This project has adopted the [Contributor Covenant Code of Conduct](CODE-OF-CONDUCT.pdf). For more information see the [Code of Conduct FAQ](https://www.contributor-covenant.org/faq/).

## License

Copyright (c) Georgia Tech Research Institute. All rights reserved.

Licensed under the [GPL v3](LICENSE.md) license.

## Security

If you believe you have found a security vulnerability in the GPSSample application, we encourage you to **_responsibly disclose this and NOT open a public issue_**. We will investigate all legitimate reports. Email `gpssample23@gmail.com` to disclose any security vulnerabilities.

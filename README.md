# Ampasamp
A minimal password sampler.

## Overview
This C# application will (optionally randomly) sample compliant passwords from a file according to a _task_ specified as a JSON file. Multiple policies can be sampled at once at identical sample sizes. C# is used because I wanted to write this fast and use it from Windows.

## Building
Build this application like any other Visual Studio 2017 project. Remember, though, that you'll need to target `x64` processors if you want to sample from huge banks

## Usage
Briefly, use the program like this:

```
./Ampasamp.exe -t mytask.json -d mypasswordlist.txt
```

There really aren't that many options to get to grips with.

| Option     | Shorthand | Required? | Description                        |
|------------|-----------|-----------|------------------------------------|
| --database | -d        | Yes       | The full password database to use. |
| --task     | -t        | Yes       | The task file to execute.          |
| --help     | N/A       | No        | Displays the help screen.          |

Bear in mind that this is going to write a bunch of files to your working directory. If there are any in there with the same name expect them to be overwritten without warning. Names take the following form:

```
{% task.Name %}_{% policy.Name %}_{% task.Sample %}.{% format.Extension %}
```

So a policy called `basic16` under a task called `mytask` with a sample size of `1000` with output format `coq` will generate a file called:

```
mytask_basic16_1000.v
```

## Tasks
Commented for clarity, the task file format looks like this (remove comments before using this, comments aren't valid JSON):

```javascript
{
  "name": "MySamplingTask", // The name of the task.
  "sample": 10000, // The number of passwords to sample under each policy.
  "output": "plain", // The format of any output, can be "plain", "json" or "coq".
  "cullNonAscii": true, // If set to true, removes any passwords containing non-ASCII characters.
  "cullNonPrintable": true, // If set to true, removes any passwords containing non-printable ASCII characters.
  "randomizeInitial": true, // If set to true, randomizes the data before collecting any samples.
  "randomizeEachSample": false, // If set to true, randomizes the data after collecting each sample..
  "deduplicate": false, // If set to true, removes all duplicates before sampling.
  "policies": [ // A collection of policies to sample according to.
    {
      "name": "Basic8", // The name of the policy.
      "length": 8, // The minimum password length allowed by the policy.
      "uppers": 0, // The minimum number of uppercase characters in passwords allowed by the policy.
      "lowers": 0, // The minimum number of lowercase characters in passwords allowed by the policy.
      "digits": 0, // The minimum number of digits characters in passwords allowed by the policy.
      "others": 0, // The minimum number of non-alphanumeric characters in passwords allowed by the policy.
      "classes": 0, // The minimum number of character classes present in passwords allowed by the policy.
      "words": 0 // The minimum number of words in passwords permitted under the policy.
	  "Repetitions": -1, // The maximum number of times a character in a password can be repeated.
	  "Consecutives": -1, // The maximum number of times a character in a password can vary from its predecessor by one code point.
	  "Dictionary": null // The file path of the dictionary to use for the dictionary check.
    }
  ],
}
```

## Credits
For any contributions not recognised below, please open an issue. A maintainer will resolve the issue as soon as possible.
- Software: Saul Johnson [@lambdacasserole](https://github.com/lambdacasserole)
- Dictionaries: Alexander Peslyak a.k.a. Solar Designer (see [Licensing](#licensing))

## Licensing
Licensed under the MIT license, with the exception of the wordlists under `/Ampasamp/Dictionaries` which have their own license (included, can also be found [here](http://download.openwall.net/pub/wordlists/LICENSE.html)) and [readme](http://download.openwall.net/pub/wordlists/README.html).

## Resources
For password lists, [Daniel Miessler's SecLists](https://github.com/danielmiessler/SecLists) is an exceptional resource.

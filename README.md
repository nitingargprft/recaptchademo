**reCAPTCHA v3 and reCAPTCHA v2 Fallback Integration**
This guide explains how to implement a combination of reCAPTCHA v3 and reCAPTCHA v2 (Checkbox) for user verification in your application. In this implementation, reCAPTCHA v3 is used by default, and if it fails or returns a suspicious score, reCAPTCHA v2 is displayed to ensure user verification.

**Prerequisites**
reCAPTCHA v2 and reCAPTCHA v3 site key and secret key obtained from reCAPTCHA Admin Console.

**Testing**
Test your implementation by submitting the form. The reCAPTCHA v3 token is captured on the client side and verified on the server side. If it fails or returns a suspicious score, reCAPTCHA v2 is displayed for additional verification.

**Important Note**
For testing purposes, the code provided in the example sets success = false even if the reCAPTCHA v3 score is legitimate. In a production environment, you should adjust the threshold and logic based on your application's security requirements.

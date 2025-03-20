# Carton Caps: Invite Microservice

This microservice manages referrals and redemptions for the Carton Caps application.

## Overview

The Invite microservice provides endpoints for:

| HTTP Method | Endpoint URL                                                 | Description                                                   |
|-------------|--------------------------------------------------------------|---------------------------------------------------------------|
| POST        | `/invite/api/referrals`                                      | Creates a new referral for the referrer.                      |
| POST        | `/invite/api/redemptions`                                    | Creates a new redemption for the referee.                     |
| GET         | `/invite/api/referrals`                                      | Lists all referrals for a given referrer.                     |
| GET         | `/invite/api/referrals/{referral_id}`                        | Retrieves a specific referral by its ID.                      |
| GET         | `/invite/api/redeemed-referrals`                             | Lists all redeemed referrals for a given referrer.            |
| GET         | `/invite/api/redemptions/{redemption_id}`                    | Retrieves a specific redemption by its ID.                    |
| GET         | `/invite/api/referral-link`                                  | Retrieves a referral link using the referrer's referral code. |

## Port
By default, the microservice uses port 8080 for communication. e.g. http://localhost:8080/invite/api/referrals

## Authentication

All endpoints are protected by JWT bearer token authentication.

### JWT Generation

Developers can generate valid JWTs using jwt.io's [JWT Encoder](https://jwt.io/). (Note: make sure you select __JWT Encoder__ as it defaults to JWT Decoder)

\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*

__IMPORTANT__: PLEASE USE THE FOLLOWING SECRET WHEN ENCODING A JWT: `carton-caps-secret-for-code-challenge`

\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*\*


### Example JWT Payload

```json
{
  "iss": "https://fake-auth0.local/",
  "sub": "1234567890",
  "aud": "https://fake-api/",
  "exp": 1893456000,
  "nbf": 1516239022,
  "iat": 1516239022,
  "name": "John Doe"
}
```


### Important JWT Considerations

* **Distinct sub Values:** Referral creation and redemption require separate tokens with distinct `sub` values. This ensures proper tracking of referrers and referees.
* The `name` field should also be unique to reflect the user associated with the `sub`.
* All other fields should remain consistent.
* **Expiration:** Ensure the `exp` claim is set appropriately.

## API Documentation

* **Scalar Documentation:** Access the API documentation and OpenAPI specification at `http://localhost:8080/scalar`. Additionally, an OpenAPI specification is included in the base directory of this repository as: __OpenAPI-Specification.json__

## Getting Started

1.  **Generate a JWT:** Use the provided __secret__ and __example payload__ as a template to generate a valid JWT. 
Remember to modify the `sub` and `name` fields as needed.

2.  **Include the JWT:** Include the JWT in the `Authorization` header of your requests, using the Bearer scheme. 
Example: `Authorization: Bearer <your_jwt_token>`.

3.  **Explore the API:** Refer to the Scalar documentation for details on available endpoints and request/response formats.

### Importing and Running `invite.tar` in Docker  

The `invite.tar` file is a portable Docker image that can be easily imported and run on any system with Docker installed. 

Follow the steps below to load the image into Docker and start a container from it.

#### 1. Import the Docker Image  
Use the following command to build a docker image (from the base directory of the project)
```sh
docker build --tag invite:v1 .
```

#### 2. Verify the Image  
To confirm that the image was imported correctly, list all available Docker images:  
```sh
docker images
```
Look for an image with the appropriate repository name and tag.

#### 3. Run a Container from the Image  
Start a new container using the imported image:  
```sh
docker run -d --name invite-container -p 8080:8080 invite:v1
```

#### 4. Check Running Containers  
To ensure the container is running, use:  
```sh
docker ps
```
If everything is set up correctly, the `invite-container` should be listed as running.

### 5. Access the Application  
Open your browser and navigate to:  
```
http://localhost:8080/scalar
```

### 6. Stopping and Removing the Container  
To stop the container:  
```sh
docker stop invite-container
```
To remove the container:  
```sh
docker rm invite-container
```
## Addendum

### Considerations for new API endpoints
- How will existing users create new referrals using their existing referral code?

Existing users will create new referrals using their existing referral code by POSTing to the Invite API's /referrals endpoint with the required request information. 
Their existing referral code will be fetched from the Profile Management API.

- How will the app generate referral links for the Share feature?

The app will generate referral links for the Share feature by GETing /referral-link from the Invite API, which allows application administrators to reconfigure the referral link without having to push an update to the mobile app (e.g. introducing new query params).

- How will existing users check the status of their referrals?

Existing users will check the status of their referrals by GETing /redeemed-referrals from the Invite API, which provides them with the essential information to display on their mobile device.

- How will the app know where to direct new users after they install the app via a referral?

The app will know where to direct new users after they install the app via a referral by utilizing the shareable deferred deep links.

- Since users may eventually earn rewards for referrals, should we take extra steps to mitigate abuse?

We should absolutely take extra steps to mitigate abuse. Future updates to the Invite API should focus on supporting additional UTM params (e.g. medium, campaign, etc.), IP address tracking, rate limiting requests, capping no. of referrals per day/week/month, potentially include geolocation data if available, anamoly detection, device fingerprinting if available, velocity checking and many, many more enhancements for robust security.

### Other Considerations
- Do referral codes expire?
- Does a user have one-and-only-one referral code?
- Is there a reason to support invalidation a referral code and changing it for a user?
- What happens if a referrer is deleted? Are referral codes recycled? Do we need to maintain relationship between referral and code?
- Do we need to support a referral-centric system? (currently this is a referrer-centric system, i.e. we can connect a redemption to a referrer)

### Known limitations
- appsettings.json integration
- E2E testing
- Logging
- Pagination
- Custom analytics

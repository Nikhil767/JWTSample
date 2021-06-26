# JWTSample
JWT with .NetCore Sample, For Swagger 'Swashbuckle.AspNetCore' is used (6.1.4)
- By default 3 user's are configured inside `JWTSample.DataContext.SampleDbContext` which are as follows :
- 1. **admin** - for admin user token will be of 45 Minutes, (password are same as username)
- 2. **test** - for test user token will be of 5 Minutes, (password are same as username)
- 3. **user** - for use user token will be of 5 Minutes, (password are same as username)

** JWT Token Expiry is based on Role in this sample `appsettings.Development.json`**
```json
  "JWTTokenConfig": {
	"AdminExpiry": 45,
	"Aud": "JWT.Sample.API",
	"Iss": "JWT.Service",
	"Secret": "MySecretKeyisverylongenough@!()",
	"UserExpiry": 5
  }
```

***Generate JWT Token:***
```csharp
	var tokenDescriptor = new SecurityTokenDescriptor
	{
		Issuer = issuer,
		Audience = audience,                
		Subject = new ClaimsIdentity(new[] { new Claim("userName", userName) }),
		Expires = DateTime.UtcNow.AddMinutes(expiry),
		SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
	};
	var tokenHandler = new JwtSecurityTokenHandler();
	var securityToken = tokenHandler.CreateToken(tokenDescriptor);
	var originalToken = tokenHandler.WriteToken(securityToken);
```

***Validate JWT Token inside ConfigureServices method:***
```csharp
	services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		var aud = Configuration.GetSection("JWTTokenConfig:Aud").Value;
		var iss = Configuration.GetSection("JWTTokenConfig:Iss").Value;
		var secretKey = Encoding.ASCII.GetBytes(Configuration.GetSection("JWTTokenConfig:Secret").Value);
		var signingKey = new SymmetricSecurityKey(secretKey);
		var tokenValidationParam = new TokenValidationParameters
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = iss,
			ValidAudience = aud,
			IssuerSigningKey = signingKey,
			ValidateLifetime = true,
			RequireExpirationTime = true,
			ClockSkew = TimeSpan.Zero
		};
		opt.RequireHttpsMetadata = false;                    
		opt.TokenValidationParameters = tokenValidationParam;
	});
```

***Use of Authentication inside Configure method:***
```csharp
	app.UseAuthentication();
```


***Configure Swagger for API's inside ConfigureServices method:***
```csharp
	services.AddSwaggerGen(s =>
	{
		s.SwaggerDoc("v1", new OpenApiInfo
		{
			Version = "1.0",
			Title = "JWT Sample APi",
			Description = "JWT Authentication APi Demo"
		});

		s.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
		{
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.ApiKey,
			Name = "Authorization",
			Description = "Please enter 'Bearer' followed by JWT Token value",
			Scheme = JwtBearerDefaults.AuthenticationScheme
		});

		s.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Name = JwtBearerDefaults.AuthenticationScheme,
					Description = "Please enter 'Bearer' followed by JWT Token value",
					Scheme = "oauth2",
					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				}, Enumerable.Empty<string>().ToList()
			}
		});
	});
```

***Use Swagger Configuration inside Configure method:***
```csharp
	app.UseSwagger().UseSwaggerUI();
```

# How to Run & Test API' with Swagger
1.	Clone the Repository & Run Application Directly
2.	Hit Login api from Swagger by passing username : `test` & password : `test`
3.	It will return jwt token in response if username & password is valid, otherwise 400 with exception message
4.	click on Authorize button on swagger page, & put `Bearer JWT Token value` & execute API

# How to Run & Test API' with postman
1.	Clone the Repository & Run Application Directly
2.	Hit Login api from Postman
	```json
	URL : http://localhost:58591/api/login
	Request Type : POST
	payload : 
	{
		"UserName":"test",
		"Password":"test"
	}
	```
3.	It will return jwt token in response if username & password is valid, otherwise 400 with exception message
4.	copy the token from login api response & create a GET request and pass token with bearer keyword before token in Header with `Authorization` key
	```json
	URL : http://localhost:58591/api/User/test
	Request Type : Get
	Header : 
	key => Authorization
	value => Bearer JWTTokenValue
	```
	
###### For more details about JWT please visit [JWT](https://jwt.io/)

###### For JWT token Decode please visit [JWT](https://jwt.ms/)
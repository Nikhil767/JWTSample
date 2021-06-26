# JWTSample
JWT with .NetCore Sample

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
using Modelo.entidades;

namespace WebApi.Extensions;
public static class PoliciesConfiguration
{
    public static IServiceCollection AddPoliciesServices(
            this IServiceCollection services
        )
    {
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(
                PolicyMaster.TRANSPORTISTA_READ, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.TRANSPORTISTA_READ
                    )
                   )
            );

            opt.AddPolicy(
                PolicyMaster.TRANSPORTISTA_WRITE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.TRANSPORTISTA_WRITE
                    )
                   )
            );

            opt.AddPolicy(
                PolicyMaster.TRANSPORTISTA_UPDATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.TRANSPORTISTA_UPDATE
                    )
                   )
            );
            opt.AddPolicy(
              PolicyMaster.VIAJE_READ, policy =>
                 policy.RequireAssertion(
                  context => context.User.HasClaim(
                  c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.VIAJE_READ
                  )
                 )
          );
            opt.AddPolicy(
             PolicyMaster.VIAJE_UPDATE, policy =>
                policy.RequireAssertion(
                 context => context.User.HasClaim(
                 c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.VIAJE_UPDATE
                 )
                )
         );
            opt.AddPolicy(
                PolicyMaster.VIAJE_CREATE, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.VIAJE_CREATE
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.VIAJEDETALLE_READ, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.VIAJEDETALLE_READ
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.VIAJEDETALLE_UPDATE, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.VIAJEDETALLE_UPDATE
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.VIAJEDETALLE_CREATE, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.VIAJEDETALLE_CREATE
                    )
                    )
                );
            opt.AddPolicy(
                PolicyMaster.SUCURSALCOLABORADOR_READ, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.SUCURSALCOLABORADOR_READ
                    )
                    )
                );
            opt.AddPolicy(
                PolicyMaster.SUCURSALCOLABORADOR_UPDATE, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.SUCURSALCOLABORADOR_UPDATE
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.SUCURSALCOLABORADOR_CREATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.SUCURSALCOLABORADOR_CREATE
                    )
                   )
            );

            opt.AddPolicy(
                PolicyMaster.SUCURSAL_READ, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.SUCURSAL_READ
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.SUCURSAL_UPDATE, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                        c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.SUCURSAL_UPDATE
                        )
                    )
                );
            opt.AddPolicy(
                PolicyMaster.SUCURSAL_CREATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.SUCURSAL_CREATE
                    )
                   )
            );
            opt.AddPolicy(
                PolicyMaster.COLABORADOR_READ, policy =>
                    policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COLABORADOR_READ
                    )
                    )
            );
            opt.AddPolicy(
                PolicyMaster.COLABORADOR_UPDATE, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                        c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COLABORADOR_UPDATE
                        )
                    )
                );
            opt.AddPolicy(
                PolicyMaster.COLABORADOR_CREATE, policy =>
                   policy.RequireAssertion(
                    context => context.User.HasClaim(
                    c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.COLABORADOR_CREATE
                    )
                   )
            );
        }


        );




        return services;
    }
}

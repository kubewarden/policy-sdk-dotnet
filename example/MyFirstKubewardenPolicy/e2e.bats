#!/usr/bin/env bats

@test "reject because defaultBackend is set" {
  run kwctl run annotated-policy.wasm -r test_data/ingress_create_with_defaultBackend.json

  # this prints the output when one the checks below fails
  echo "output = ${output}"

  # request rejected
  [ "$status" -eq 0 ]
  [ $(expr "$output" : '.*allowed.*false') -ne 0 ]
  [ $(expr "$output" : '.*Ingress defaultBackend must not be set.*') -ne 0 ]

}

@test "accept becauses defaultBackend is not set" {
  run kwctl run annotated-policy.wasm -r test_data/ingress_create.json
  # this prints the output when one the checks below fails
  echo "output = ${output}"

  # request accepted
  [ "$status" -eq 0 ]
  [ $(expr "$output" : '.*allowed.*true') -ne 0 ]
}

@test "accept and mutate when defaultBackend is not set and policy is allowed to mutate" {
  run kwctl run annotated-policy.wasm \
    -r test_data/ingress_create_with_defaultBackend.json \
    --settings-json '{"wipe_default_backend": true}'
  # this prints the output when one the checks below fails
  echo "output = ${output}"

  # request accepted
  [ "$status" -eq 0 ]
  [ $(expr "$output" : '.*allowed.*true') -ne 0 ]
  [ $(expr "$output" : '.*patch.*W3sib3AiOiJyZW1vdmUiLCJwYXRoIjoiL3NwZWMvZGVmYXVsdEJhY2tlbmQifV0=') -ne 0 ]
}

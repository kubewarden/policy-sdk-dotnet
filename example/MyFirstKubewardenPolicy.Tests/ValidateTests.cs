namespace MyFirstKubewardenPolicy.Tests;
using Policy;
using System.Text.Json;
using KubewardenPolicySDK;
using k8s.Models;

public class ValidateTests
{
  [TestCase(true, false)]
  [TestCase(true, true)]
  [TestCase(false, true)]
  [TestCase(false, false)]
  public void TestValidation(bool hasDefaultBackend, bool enableMutation)
  {
    var settings = new PolicySettings
    {
      WipeDefaultBackend = enableMutation,
    };

    var ingress = CreateIngress(hasDefaultBackend);
    var reqPayload = CreateValidationPayload(ingress, settings);

    var validationRespBytes = Validator.Validate(reqPayload);
    var validationResp = JsonSerializer.Deserialize<ValidationResponse>(validationRespBytes);

    if (validationResp is ValidationResponse res)
    {
      if (hasDefaultBackend)
      {
        if (enableMutation)
        {
          Assert.IsTrue(
            res.Accepted,
            "Should have been mutated and accepted"
          );
          Assert.IsNotNull(res.MutatedObject);
        }
        else
        {
          Assert.IsFalse(
            res.Accepted,
            "Ingress objects with a default backend should be rejected"
          );
          Assert.IsNull(res.MutatedObject);
        }
      }
      else
      {
        Assert.IsTrue(
          res.Accepted,
          "Ingress objects without a default backend should be accepted"
        );
        Assert.IsNull(res.MutatedObject);
      }

    }
    else
    {
      Assert.Fail("ValidationRespose is null");
    }
  }

  private static V1Ingress CreateIngress(bool hasDefaultBackend)
  {
    var paths = new List<V1HTTPIngressPath>();
    paths.Add(new V1HTTPIngressPath
    {
      Path = "/testpath",
      PathType = "Prefix",
      Backend = new V1IngressBackend
      {
        Service = new V1IngressServiceBackend
        {
          Name = "test",
          Port = new V1ServiceBackendPort
          {
            Name = "http",
            Number = 80,
          }
        }
      }
    });

    var rules = new List<V1IngressRule>();
    rules.Add(new V1IngressRule
    {
      Http = new V1HTTPIngressRuleValue
      {
        Paths = paths,
      }
    });

    V1IngressSpec ingressSpec;
    if (hasDefaultBackend)
    {
      ingressSpec = new V1IngressSpec
      {
        Rules = rules,
        DefaultBackend = new V1IngressBackend
        {
          Resource = new V1TypedLocalObjectReference
          {
            ApiGroup = "k8s.example.com",
            Kind = "StorageBucket",
            Name = "static-assets",
          }
        }
      };
    }
    else
    {
      ingressSpec = new V1IngressSpec
      {
        Rules = rules,
      };
    }

    return new V1Ingress
    {
      Metadata = new V1ObjectMeta
      {
        Name = "test-ingress",
      },
      Spec = ingressSpec,
    };
  }

  private static byte[] CreateValidationPayload(V1Ingress ingress, PolicySettings policySettings)
  {
    var ingressJson = JsonSerializer.Serialize(ingress);
    JsonDocument ingressJsonDoc = JsonDocument.Parse(ingressJson);

    return CreateValidationPayload(ingressJsonDoc, policySettings);
  }

  private static byte[] CreateValidationPayload(JsonDocument obj, PolicySettings policySettings)
  {
    var kubeAdmissionReq = new KubernetesAdmissionRequest
    {
      Uid = "not relevant",
      Kind = new GroupVersionKind
      {
        Group = "not relevant",
        Kind = "not relevant",
        Version = "not relevant",
      },
      Resource = new GroupVersionResource
      {
        Group = "not relevant",
        Kind = "not relevant",
        Version = "not relevant",
      },
      Object = obj,
    };

    var policySettingsJson = JsonSerializer.Serialize(policySettings);
    var policySettingsJsonDoc = JsonDocument.Parse(policySettingsJson);

    var request = new ValidationRequest
    {
      Settings = policySettingsJsonDoc,
      Request = kubeAdmissionReq,
    };

    return JsonSerializer.SerializeToUtf8Bytes(request);
  }
}
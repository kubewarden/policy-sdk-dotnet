
using k8s.Models;
using KubewardenPolicySDK;
using System.Text.Json;


namespace Policy
{
    public class PolicyRules
    {
        public static byte[] Validate(byte[] payload)
        {
            try
            {
                ValidationRequest? validationRequest = JsonSerializer.Deserialize<ValidationRequest>(payload);

                if (validationRequest is ValidationRequest req)
                {
                    PolicySettings? policySettings = req.Settings.Deserialize<PolicySettings>();

                    return ProcessValidationRequest(ref req, policySettings);
                }
                else
                {
                    return Kubewarden.RejectRequest("Invalid payload", 400, null, null);
                }

            }
            catch (Exception e)
            {
                return Kubewarden.RejectRequest($"Internal errror: {e}", 500, null, null);
            }
        }


        private static byte[] ProcessValidationRequest(ref ValidationRequest req, PolicySettings ps)
        {
            V1Pod maybePod = Kubewarden.GetResource<V1Pod>(req.Request.Namespace, "iliketobealone", true);
            if(maybePod != null)
            {
                return Kubewarden.RejectRequest("A Pod that wants to be alone already exists!", 400, null, null);
            }

            KubernetesList<V1Namespace> nses = Kubewarden.ListResourcesAll<V1Namespace>("", "");
            if(nses == null)
            {
                return Kubewarden.RejectRequest($"Internal errror", 500, null, null);
            }
            foreach(var ns in nses.Items)
            {
                Console.WriteLine($"Looking at namespace: {ns.Name()}");
                //Ideally you would GetResource the actual namespace instead of all of them, but for demo purposes lets look at them all
                if (ns.Name() == req.Request.Namespace && ns.Labels()?.ContainsKey("nopodsplease") == true)
                {
                    return Kubewarden.RejectRequest("The namespace doesn't want any pods!", 400, null, null);
                }
            }

            KubernetesList<V1ConfigMap> configMaps = Kubewarden.ListResourcesAll<V1ConfigMap>("", "");
            if(configMaps == null)
            {
                return Kubewarden.RejectRequest($"Internal errror", 500, null, null);
            }
            foreach(var cm in configMaps.Items)
            {
                Console.WriteLine($"Looking at configmap: {cm.Name()}");
                if(cm.Name().Contains("nopodsplease") == true)
                {
                    return Kubewarden.RejectRequest("The configmap told us no pods!", 400, null, null);
                }
            }



            return Kubewarden.AcceptRequest();
        }


    }

}

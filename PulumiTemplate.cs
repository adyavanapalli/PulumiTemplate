using Pulumi;
using Pulumi.AzureNative.Compute;
using AzureNative = Pulumi.AzureNative;

/// <summary>
/// TODO: Needs documentation.
/// </summary>
public class PulumiTemplate : Stack
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public PulumiTemplate()
    {
        var commonResourceSuffix = "PulumiTemplate-eastus";

        var resourceGroup = new AzureNative.Resources.ResourceGroup(nameof(AzureNative.Resources.ResourceGroup),
                                                                    new()
                                                                    {
                                                                        ResourceGroupName = $"rg-{commonResourceSuffix}"
                                                                    });

        var virtualNetwork = new AzureNative.Network.VirtualNetwork(nameof(AzureNative.Network.VirtualNetwork),
                                                                    new()
                                                                    {
                                                                        VirtualNetworkName = $"vnet-{commonResourceSuffix}",
                                                                        ResourceGroupName = resourceGroup.Name,
                                                                        AddressSpace = new AzureNative.Network.Inputs.AddressSpaceArgs
                                                                        {
                                                                            AddressPrefixes = new[]
                                                                            {
                                                                                "10.0.0.0/29"
                                                                            }
                                                                        }
                                                                    });

        var subnet = new AzureNative.Network.Subnet($"snet-{commonResourceSuffix}",
                                                    new()
                                                    {
                                                        AddressPrefix = "10.0.0.0/29",
                                                        ResourceGroupName = resourceGroup.Name,
                                                        VirtualNetworkName = virtualNetwork.Name,
                                                    });

        var networkInterface = new AzureNative.Network.NetworkInterface(nameof(AzureNative.Network.NetworkInterface),
                                                                        new AzureNative.Network.NetworkInterfaceArgs
                                                                        {
                                                                            IpConfigurations = new()
                                                                            {
                                                                                new AzureNative.Network.Inputs.NetworkInterfaceIPConfigurationArgs
                                                                                {
                                                                                    Name = $"ipc-{commonResourceSuffix}",
                                                                                    Subnet = new AzureNative.Network.Inputs.SubnetArgs
                                                                                    {
                                                                                        Id = subnet.Id
                                                                                    }
                                                                                }
                                                                            },
                                                                            NetworkInterfaceName = $"nic-{commonResourceSuffix}",
                                                                            ResourceGroupName = resourceGroup.Name,
                                                                        });

        var networkSecurityGroup = new AzureNative.Network.NetworkSecurityGroup(nameof(AzureNative.Network.NetworkSecurityGroup),
                                                                                new AzureNative.Network.NetworkSecurityGroupArgs
                                                                                {
                                                                                    NetworkSecurityGroupName = $"nsg-{commonResourceSuffix}",
                                                                                    ResourceGroupName = resourceGroup.Name
                                                                                });

        var virtualMachine = new AzureNative.Compute.VirtualMachine(nameof(AzureNative.Compute.VirtualMachine),
                                                                    new AzureNative.Compute.VirtualMachineArgs
                                                                    {
                                                                        HardwareProfile = new AzureNative.Compute.Inputs.HardwareProfileArgs
                                                                        {
                                                                            VmSize = "Standard_B1ls"
                                                                        },
                                                                        NetworkProfile = new AzureNative.Compute.Inputs.NetworkProfileArgs
                                                                        {
                                                                            NetworkInterfaces = new()
                                                                            {
                                                                                new AzureNative.Compute.Inputs.NetworkInterfaceReferenceArgs
                                                                                {
                                                                                    DeleteOption = DeleteOptions.Delete,
                                                                                    Id = networkInterface.Id,
                                                                                    Primary = true
                                                                                }
                                                                            }
                                                                        },
                                                                        OsProfile = new AzureNative.Compute.Inputs.OSProfileArgs
                                                                        {
                                                                            AdminPassword = $"vm-{commonResourceSuffix}",
                                                                            AdminUsername = $"vm-{commonResourceSuffix}",
                                                                            ComputerName = $"vm-{commonResourceSuffix}"
                                                                        },
                                                                        ResourceGroupName = resourceGroup.Name,
                                                                        StorageProfile = new AzureNative.Compute.Inputs.StorageProfileArgs
                                                                        {
                                                                            ImageReference = new AzureNative.Compute.Inputs.ImageReferenceArgs
                                                                            {
                                                                                Offer = "0001-com-ubuntu-server-hirsute",
                                                                                Publisher = "Canonical",
                                                                                Sku = "21_04-gen2",
                                                                                Version = "latest",
                                                                            },
                                                                        },
                                                                        VmName = $"vm-{commonResourceSuffix}"
                                                                    });
    }
}

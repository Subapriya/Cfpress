<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.ascx.cs" Inherits="Governor.Umbraco.uShare.UI.Dashboard.Dashboard" %>

<style>
    .uShare .dashboardWrapper h3 {
        margin: 15px 0 2px 0;
    }
    .uShare .auth-button, .uShare .deauth-button {
        width: 150px;
    }
    .uShare .deauth-button {
        margin-left: 10px;
    }
</style>

<script type="text/javascript">
    function openFacebookAuthPopup() {
        window.open('/umbraco/plugins/ushare/usharefacebook.aspx', '_blank', 'width=450, height=260');
        //UmbClientMgr.mainWindow().UmbSpeechBubble.ShowMessage('success', 'Facebook Authorization <br /> complete', '');
    }

    function openLinkedInAuthPopup() {
        window.open('/umbraco/plugins/ushare/usharelinkedin.aspx', '_blank', 'width=440, height=572');
        //UmbClientMgr.mainWindow().UmbSpeechBubble.ShowMessage('success', 'LinkedIn Authorization <br /> complete', '');
    }

    function openTwitterAuthPopup() {
        window.open('/umbraco/plugins/ushare/usharetwitter.aspx', '_blank', 'width=385, height=640');
        //UmbClientMgr.mainWindow().UmbSpeechBubble.ShowMessage('success', 'Twitter Authorization <br /> complete', '');
    }
</script>

<div class="uShare">
    <div class="propertypane">
        <div class="propertyItem">
            <div class="dashboardWrapper">
                <h2>uShare Authorization / Deauthorization</h2>
                <img src="/umbraco/plugins/uShare/icon.png" alt="uShare" class="dashboardIcon" />
                <p>
                    Authorize or deauthorize the current Umbraco user to share content to the specific service providers.<br />
                    Please note that only single service provider access tokens are stored in association with Umbraco users.
                </p>

                <div class="service-providers">
                    <asp:MultiView runat="server" ID="mvServiceProviders">
                        <asp:View runat="server" ID="viewServiceProviders">
                            <asp:PlaceHolder runat="server" ID="phFacebook" Visible="False">
                                <div class="service-provider">
                                    <h3>Facebook</h3>
                                    <asp:Button runat="server" ID="btnFacebookAuthorize" Text="Facebook Authorize" CssClass="auth-button" OnClientClick="openFacebookAuthPopup(); return false;" />
                                    <asp:Button runat="server" ID="btnFacebookDeauthorize" Text="Facebook Deauthorize" CssClass="deauth-button" OnClick="btnFacebookDeauthorize_Click" />
                                </div>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="phLinkedIn" Visible="False">
                                <div class="service-provider">
                                    <h3>LinkedIn</h3>
                                    <asp:Button runat="server" ID="btnLinkedInAuthorize" Text="LinkedIn Authorize" CssClass="auth-button" OnClientClick="openLinkedInAuthPopup(); return false;" />
                                    <asp:Button runat="server" ID="btnLinkedInDeauthorize" Text="LinkedIn Deauthorize" CssClass="deauth-button" OnClick="btnLinkedInDeauthorize_Click" />
                                </div>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="phTwitter" Visible="False">
                                <div class="service-provider">
                                    <h3>Twitter</h3>
                                    <asp:Button runat="server" ID="btnTwitterAuthorize" Text="Twitter Authorize" CssClass="auth-button" OnClientClick="openTwitterAuthPopup(); return false;" />
                                    <asp:Button runat="server" ID="btnTwitterDeauthorize" Text="Twitter Deauthorize" CssClass="deauth-button" OnClick="btnTwitterDeauthorize_Click" />
                                </div>
                            </asp:PlaceHolder>
                        </asp:View>
                        <asp:View runat="server" ID="viewNoServiceProviders">
                            <p>No active service providers. See data type properties to activate.</p>
                        </asp:View>
                    </asp:MultiView>
                </div>

            </div>
        </div>
    </div>
</div>






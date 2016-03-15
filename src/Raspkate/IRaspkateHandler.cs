// -------------------------------------------------------------------------------------------------------
//
//                                                       jSL                                        
//   j2S0Z0S7,                                          ,@BE                                        
//  ;@@@@B@B@B@                                         @@@.                      @B@               
//  O@@i    B@BL     rv7   i:    .rujr.   .:   i7r.     B@B    i7:   .iLr  .:,  ,LB@@.i     ,LJL:   
//  @B@    :@B@   ,@@B@B@M@B@  :@@@B@@@   @B@Z@B@@@G   r@BY  P@@@, :@B@B@BOB@E 0@B@B@B@i  PB@@@B@@5 
// rB@BN0NB@B0   :B@B,  :@@@.  @B@       iB@@@.  @B@.  @@@ .B@Bi  ;@B@.  rB@B    @@@     @B@    B@B 
// B@B@@@B@B     B@B     B@B   @@BB7     O@BU    B@B.  B@BOB@:    @@@     @B@   :B@2    @B@OuX@B@@i 
// @B@   1B@M   r@BX    1@Bu    :0B@B@   @B@     @@@  L@Br2@BL   u@@J    ZB@7   B@B    ,B@MYP0j:    
//YB@8    @B@.  U@@X   G@B@        @@B  rB@M    @B@.  @B@  B@@J  P@Bu   @B@B    @B@    ;@@0         
//@@Bi    B@@@   @@@@@@@B@B  @@@qN@@B7  B@B@B@B@B@:  :B@B   @@BL ,B@B@B@B@B@   .@@@@B1  B@B@ZO@@B.  
//0k2     iFPY    5O@k. uk:  ,XB@BZ;    @B@ LB@Pi    :0k:   ,SNj  .1MBF  1X.    :5MMq    iF@@BBS:   
//                                     uB@J                                                         
//                                     M@B
//
// Raspkate Service by daxnet, 2016
// https://github.com/daxnet/raspkate
// Licensed under GPL v2.0
// -------------------------------------------------------------------------------------------------------

using System.Net;

namespace Raspkate
{
    /// <summary>
    /// Represents that the implemented classes are Raspkate HTTP handlers.
    /// </summary>
    public interface IRaspkateHandler
    {
        /// <summary>
        /// Gets the name of the handler.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }
        /// <summary>
        /// Returns a <see cref="bool"/> value which indicates whether the current handler
        /// can handle the given HTTP request.
        /// </summary>
        /// <param name="request">The request object to be validated.</param>
        /// <returns><c>True</c> if current handler can handle the given request, otherwise, <c>False</c>.</returns>
        bool ShouldHandle(HttpListenerRequest request);

        /// <summary>
        /// Processes the given request and returns the response.
        /// </summary>
        /// <param name="request">The request to be processed by current handler.</param>
        /// <param name="response">The response which contains the returned data.</param>
        HandlerProcessResult Process(HttpListenerRequest request);

        void OnRegistering();
        void OnUnregistered();

    }
}

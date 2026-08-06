const RENDER_ORIGIN = "https://iliyaahmadi.onrender.com";
const STATIC_PATH_PREFIXES = ["/images/", "/files/", "/css/", "/js/", "/lib/"];

function isStaticRequest(request, pathname) {
    return (request.method === "GET" || request.method === "HEAD")
        && (STATIC_PATH_PREFIXES.some(prefix => pathname.startsWith(prefix))
            || pathname === "/favicon.ico");
}

function rewriteLocation(headers, upstreamUrl, publicUrl) {
    const location = headers.get("Location");
    if (!location) return;

    const redirectUrl = new URL(location, upstreamUrl);
    if (redirectUrl.host !== upstreamUrl.host) return;

    redirectUrl.protocol = publicUrl.protocol;
    redirectUrl.host = publicUrl.host;
    headers.set("Location", redirectUrl.toString());
}

export default {
    async fetch(request, environment, context) {
        const publicUrl = new URL(request.url);
        const upstreamUrl = new URL(publicUrl.pathname + publicUrl.search, RENDER_ORIGIN);
        const cacheable = isStaticRequest(request, publicUrl.pathname);

        if (cacheable) {
            const cachedResponse = await caches.default.match(request);
            if (cachedResponse) return cachedResponse;
        }

        const headers = new Headers(request.headers);
        headers.set("X-Forwarded-Host", publicUrl.host);
        headers.set("X-Forwarded-Proto", "https");

        const clientIp = request.headers.get("CF-Connecting-IP");
        if (clientIp) headers.set("X-Forwarded-For", clientIp);

        headers.delete("CF-Connecting-IP");
        headers.delete("CF-IPCountry");
        headers.delete("CF-Ray");
        headers.delete("CF-Visitor");

        const requestInit = {
            method: request.method,
            headers,
            redirect: "manual"
        };

        if (request.method !== "GET" && request.method !== "HEAD") {
            requestInit.body = await request.arrayBuffer();
        }

        const upstreamResponse = await fetch(upstreamUrl, requestInit);
        const responseHeaders = new Headers(upstreamResponse.headers);
        rewriteLocation(responseHeaders, upstreamUrl, publicUrl);

        if (cacheable && upstreamResponse.ok) {
            responseHeaders.set(
                "Cache-Control",
                "public, max-age=86400, stale-while-revalidate=604800"
            );
        }

        const response = new Response(upstreamResponse.body, {
            status: upstreamResponse.status,
            statusText: upstreamResponse.statusText,
            headers: responseHeaders
        });

        if (cacheable && response.ok) {
            context.waitUntil(caches.default.put(request, response.clone()));
        }

        return response;
    }
};

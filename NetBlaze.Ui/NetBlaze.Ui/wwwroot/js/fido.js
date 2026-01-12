// ===== Helpers (REQUIRED) =====
function base64urlToBase64(base64url) {
    let base64 = base64url.replace(/-/g, '+').replace(/_/g, '/');
    while (base64.length % 4) {
        base64 += '=';
    }
    return base64;
}

function toUint8Array(base64url) {
    const base64 = base64urlToBase64(base64url);
    const binaryString = window.atob(base64);
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    return bytes;
}

function arrayBufferToBase64URL(buffer) {
    const bytes = new Uint8Array(buffer);
    let binary = '';
    bytes.forEach(b => binary += String.fromCharCode(b));
    const base64 = window.btoa(binary);
    return base64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');
}

// ===== Main FIDO function =====
window.startFidoRegister = async (options) => {
    options.challenge = toUint8Array(options.challenge);
    options.user.id = toUint8Array(options.user.id);

    options.excludeCredentials?.forEach(c => {
        c.id = toUint8Array(c.id);
    });

    const credential = await navigator.credentials.create({
        publicKey: options
    });

    return {
        id: credential.id,
        rawId: arrayBufferToBase64URL(credential.rawId),
        type: credential.type,
        response: {
            attestationObject: arrayBufferToBase64URL(
                credential.response.attestationObject),
            clientDataJSON: arrayBufferToBase64URL(
                credential.response.clientDataJSON)
        }
    };
};
window.startFidoLogin = async (options) => {
    options.challenge = toUint8Array(options.challenge);

    options.allowCredentials?.forEach(c => {
        c.id = toUint8Array(c.id);
    });

    const assertion = await navigator.credentials.get({
        publicKey: options
    });

    return {
        id: assertion.id,
        rawId: arrayBufferToBase64URL(assertion.rawId),
        type: assertion.type,
        response: {
            authenticatorData: arrayBufferToBase64URL(
                assertion.response.authenticatorData),
            clientDataJSON: arrayBufferToBase64URL(
                assertion.response.clientDataJSON),
            signature: arrayBufferToBase64URL(
                assertion.response.signature),
            userHandle: assertion.response.userHandle
                ? arrayBufferToBase64URL(assertion.response.userHandle)
                : null
        }
    };
};

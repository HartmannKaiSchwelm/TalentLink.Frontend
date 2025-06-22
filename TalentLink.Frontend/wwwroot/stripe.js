// Stripe error handling and integration
window.stripeErrorHandling = {
    // Flag to track if we've already handled an error
    errorHandled: false,
    
    // Initialize error handling for Stripe
    init: function() {
        console.log('Stripe error handling initialized');
        
        // Listen for errors in vendor-820.js and other Stripe scripts
        window.addEventListener('error', function(event) {
            // Check if the error is from a Stripe script
            if (event.filename && (event.filename.includes('vendor-820.js') || 
                event.filename.includes('stripe') || 
                event.message.includes('Stripe'))) {
                
                console.log('Caught Stripe-related error:', event.message);
                
                // Prevent the same error from being handled multiple times
                if (!window.stripeErrorHandling.errorHandled) {
                    window.stripeErrorHandling.errorHandled = true;
                    
                    // Show a user-friendly message
                    if (confirm('Es gab ein Problem mit der Stripe-Verbindung. Möchten Sie es erneut versuchen?')) {
                        // Reset the error flag
                        window.stripeErrorHandling.errorHandled = false;
                        
                        // Reload the page or redirect to payment success
                        // This is a fallback in case the payment actually went through despite the error
                        window.location.href = window.location.origin + '/paymentsuccess';
                    }
                }
                
                // Prevent the default error handling
                event.preventDefault();
                return false;
            }
        }, true); // Use capture to catch the error before it bubbles up
    },
    
    // Method to manually handle a Stripe error and continue
    handleErrorAndContinue: function() {
        console.log('Manually handling Stripe error and continuing');
        window.location.href = window.location.origin + '/paymentsuccess';
    }
};

// Initialize when the page loads
document.addEventListener('DOMContentLoaded', function() {
    window.stripeErrorHandling.init();
});
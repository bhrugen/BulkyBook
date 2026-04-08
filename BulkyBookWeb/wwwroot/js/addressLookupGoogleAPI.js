async function initAddressAutocomplete(streetAddressId, cityId, stateId, postalCodeId) {

    const inputAddress = document.getElementById(streetAddressId);
    const inputCity = document.getElementById(cityId);
    const inputState = document.getElementById(stateId);
    const inputPostalCode = document.getElementById(postalCodeId);

    if (!inputAddress) {
        console.error(`Street address field with ID '${streetAddressId}' not found`);
        return;
    }


    // Create the input HTML element, and append it.
    const placeAutocomplete = new google.maps.places.PlaceAutocompleteElement({});

    inputAddress.parentNode.insertBefore(placeAutocomplete, inputAddress);
    inputAddress.style.display = 'none';

    placeAutocomplete.addEventListener('gmp-select', async ({ placePrediction }) => {
        const place = placePrediction.toPlace();
        await place.fetchFields({ fields: ['addressComponents'] });

        const addressComponents = place.addressComponents;

        // Reset fields
         inputAddress.value = '';
        if (inputCity) inputCity.value = '';
        if (inputState) inputState.value = '';
        if (inputPostalCode) inputPostalCode.value = '';

        if (addressComponents) {
            let streetNumber = '';
            let route = '';

            addressComponents.forEach(component => {
                const types = component.types;

                if (types.includes('street_number')) {
                    streetNumber = component.longText;
                }
                if (types.includes('route')) {
                    route = component.longText;
                }
                if (types.includes('locality')) {
                    inputCity.value = component.longText;
                }
                if (types.includes('administrative_area_level_1')) {
                    inputState.value = component.longText;
                }
                if (types.includes('postal_code')) {
                    inputPostalCode.value = component.longText;
                }

            });


            inputAddress.value = [streetNumber, route].filter(Boolean).join(' ');
        }



    });
}
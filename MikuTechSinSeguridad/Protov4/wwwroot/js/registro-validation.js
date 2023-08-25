// registro-validation.js
$(document).ready(function () {
    // Bloquear ingreso de números en los campos con clase "bloquear-numeros"
    $('.bloquear-numeros').keypress(function (e) {
        var keyCode = e.which ? e.which : e.keyCode;
        if (keyCode >= 48 && keyCode <= 57) {
            // Bloquear tecla si es un número (código ASCII)
            e.preventDefault();
        }
    });

    // Bloquear ingreso de letras en el campo de teléfono
    $('#telefono_cliente').keypress(function (e) {
        var keyCode = e.which ? e.which : e.keyCode;
        if ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122)) {
            // Bloquear tecla si es una letra (código ASCII)
            e.preventDefault();
        }
    });
    // Mostrar mensaje de error si el teléfono no cumple con los requisitos
    $('#telefono_cliente').on('input', function () {
        var telefono = $(this).val();
        var telefonoSpan = $('#telefono_error');

        if (telefono.length === 0) {
            telefonoSpan.text('');
        } else if (!/^[0-9]*$/.test(telefono)) {
            // Eliminar caracteres no numéricos
            $(this).val(telefono.replace(/[^0-9]/g, ''));
            telefonoSpan.text('Solo se permiten números');
        } else if (!telefono.startsWith('09')) {
            telefonoSpan.text('El número debe empezar por 09');
        } else if (telefono.length !== 10) {
            telefonoSpan.text('El número debe tener 10 dígitos');
        } else {
            telefonoSpan.text('');
        }
    });

    // Borrar el mensaje de error cuando se hace clic en el campo
    $('#telefono_cliente').on('focus', function () {
        var telefonoSpan = $('#telefono_error');
        telefonoSpan.text('');
    });

    // Mostrar mensaje de error si el correo no coincide con el formato válido
    // Mostrar mensaje de error si el correo no coincide con el formato válido
    $('#correo_nuevo').on('input', function () {
        var correo = $(this).val();
        var correoErrorSpan = $('.correo-error'); // Seleccionamos el span por su clase
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (correo.length === 0) {
            correoErrorSpan.text('');
        } else if (!emailRegex.test(correo)) {
            correoErrorSpan.text('Formato correcto: example@correo.com');
        } else {
            correoErrorSpan.text('');
        }
    });

    // Borrar el mensaje de error cuando se hace clic en el campo
    $('#correo_nuevo').on('focus', function () {
        var correoErrorSpan = $('.correo-error'); // Seleccionamos el span por su clase
        correoErrorSpan.text('');
    });
});
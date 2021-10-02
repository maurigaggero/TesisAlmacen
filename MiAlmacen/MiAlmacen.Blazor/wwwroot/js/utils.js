﻿function confirmar(title, text, icon) {
    return new Promise(resolve => {
        Swal.fire({
            title,
            text,
            icon,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Cancelar',
            confirmButtonText: 'Aceptar'
        }).then((result) => {
            if (result.isConfirmed) {
                resolve(result.isConfirmed);
            }
        })
    })
}

function simple(title, text, icon) {
    Swal.fire({
        title,
        text,
        icon
    })
}

function levantaModal(id) {
    $(id).modal('show');
}

function ocultaModal(id) {
    $(id).modal('hide');
}


function levantaTooltips() {
    $('[data-toggle="tooltip"]').tooltip({
        trigger: 'hover'
    });
}

function levantaAlerta() {
    $('.alert').alert()
}

function ocultaAlerta() {
    $('.alert').alert('close')
}
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-gestion-ambiental',
  templateUrl: './gestion-ambiental.component.html',
  styleUrls: ['./gestion-ambiental.component.scss']
})
export class GestionAmbientalComponent implements OnInit {

    @Input() esVerDetalle = false;
    formGestionAmbiental: FormGroup;
    booleanosActividadRelacionada: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    tipoActividades: any[] = [
        { value: '1', viewValue: 'Manejo de materiales e insumos' },
        { value: '2', viewValue: 'Manejo de residuos de construcción y demolición' },
        { value: '3', viewValue: 'Manejo de residuos peligrosos y especiales' },
        { value: '4', viewValue: 'Otra' }
    ];

    constructor( private fb: FormBuilder )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        this.formGestionAmbiental = this.fb.group({
            actividadRelacionada: [ null ],
            tipoActividad: [ null ],
            manejoMaterialInsumo: this.fb.group({
                proveedores: this.fb.array( [
                    this.fb.group({
                        proveedor: [ '' ],
                        requirePermisosAmbientales: [ null ],
                        campoUrl: [ '' ]
                    })
                ] ),
                seEncontraronProtegidosCorrectamente: [ null ],
                requireObservacionMateriales: [ null ],
                observacionManejoMaterial: [ null ],
                urlSoporte: [ null ]
            }),
            manejoResiduosConstruccion: this.fb.group({
                esRcdCuantificado: [ null ],
                requiereObservacionSobreManejo: [ null ],
                observacionManejoResiduos: [ null ],
                gestorResiduos: this.fb.array( [
                    this.fb.group(
                        {
                            gestorResiduosConstruccion: [ '' ],
                            presentaPermisoAmbientalValido: [ null ],
                            urlSoporte: [ '' ]
                        }
                    )
                ] ),
                seReutilizaronLosResiduos: [ null ],
                cantidad: [ '' ]
            }),
            manejoResiduosPeligrosos: this.fb.group({
                seClasificaronlugarSeguro: [ null ],
                requiereObservacion: [ null ],
                obsManejoResiduosPeligrosos: [ null ],
                urlSoporte: [ '' ]
            }),
            otra: this.fb.group({
                fechaActividad: [ null ],
                observacionActividad: [ null ],
                urlSoporte: [ '' ]
            })
        });
    }

    valuePending( value: any ) {
        console.log( value );
    }

    guardar() {
        console.log( this.formGestionAmbiental.value );
    }

}

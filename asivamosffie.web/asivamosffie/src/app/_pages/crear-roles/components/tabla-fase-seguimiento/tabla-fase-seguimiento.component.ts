import { Component, ElementRef, Input, OnInit, Renderer2, ViewChild } from '@angular/core';
import { FormArray, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-fase-seguimiento',
  templateUrl: './tabla-fase-seguimiento.component.html',
  styleUrls: ['./tabla-fase-seguimiento.component.scss']
})
export class TablaFaseSeguimientoComponent implements OnInit {

    @Input() esRegistroNuevo = true;
    @Input() formFaseInicio: FormGroup;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    @ViewChild( 'matTable', { static: false, read: ElementRef } ) table: ElementRef;
    @ViewChild( 'heightAside', { static: false, read: ElementRef } ) heightAside: ElementRef;
    displayedColumns: string[] = [ 'funcionalidad', 'crear', 'modificar', 'consultar', 'eliminar' ];
    dataSource = new MatTableDataSource();

    get registros() {
        return this.formFaseInicio.get( 'registros' ) as FormArray;
    }

    constructor(
        private renderer: Renderer2,
        private fb: FormBuilder )
    { }

    ngOnInit(): void {
        const dataTable = [
            {
                funcionalidad: 'Cargar enlace del sistema de monitoreo en línea',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Visualizar avance de obra en tiempo real',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Registrar programación de personal de obra',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Registrar seguimiento diario',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Verificar seguimiento diario',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Registrar avance semanal',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Verificar avance semanal',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            }
        ]

        dataTable.forEach( registro => {
            this.registros.push( this.fb.group(
                {
                    funcionalidad: [ registro.funcionalidad, Validators.required ],
                    crear: [ null, Validators.required ],
                    modificar: [ null, Validators.required ],
                    consultar: [ null, Validators.required ],
                    eliminar: [ null, Validators.required ],
                }
            ) )
        } );

        this.dataSource = new MatTableDataSource( this.registros.controls );
        setTimeout(() => {
            this.renderer.setStyle( this.heightAside.nativeElement, 'height', `${ this.table.nativeElement.querySelector('tbody').offsetHeight }px` );
        }, 5);
    }

}

import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-fase-seguimiento',
  templateUrl: './tabla-fase-seguimiento.component.html',
  styleUrls: ['./tabla-fase-seguimiento.component.scss']
})
export class TablaFaseSeguimientoComponent implements OnInit {

    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    @ViewChild( 'matTable', { static: false, read: ElementRef } ) table: ElementRef;
    @ViewChild( 'heightAside', { static: false, read: ElementRef } ) heightAside: ElementRef;
    displayedColumns: string[] = [ 'funcionalidad', 'crear', 'modificar', 'consultar', 'eliminar' ];
    dataSource = new MatTableDataSource();

    constructor( private renderer: Renderer2 ) { }

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
        this.dataSource = new MatTableDataSource( dataTable );
        setTimeout(() => {
            this.renderer.setStyle( this.heightAside.nativeElement, 'height', `${ this.table.nativeElement.querySelector('tbody').offsetHeight }px` );
        }, 5);
    }

}

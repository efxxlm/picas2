import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-fase-cierre',
  templateUrl: './tabla-fase-cierre.component.html',
  styleUrls: ['./tabla-fase-cierre.component.scss']
})
export class TablaFaseCierreComponent implements OnInit {

    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    @ViewChild( 'matTable', { static: false, read: ElementRef } ) table: ElementRef;
    @ViewChild( 'heightAside', { static: false, read: ElementRef } ) heightAside: ElementRef;
    displayedColumns: string[] = [ 'funcionalidad', 'crear', 'modificar', 'consultar', 'eliminar' ];
    dataSource = new MatTableDataSource();

    constructor( private renderer: Renderer2 ) { }

    ngOnInit(): void {
        const dataTable = [
            {
                funcionalidad: 'Registrar informe final del proyecto',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Verificar informe final del proyecto',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Validar informe final del proyecto',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Validar cumplimiento informe final del proyecto',
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

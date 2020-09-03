import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ComiteGrilla, EstadosComite } from 'src/app/_interfaces/technicalCommitteSession';

@Component({
  selector: 'app-tabla-sesion-comite-fiduciario',
  templateUrl: './tabla-sesion-comite-fiduciario.component.html',
  styleUrls: ['./tabla-sesion-comite-fiduciario.component.scss']
})
export class TablaSesionComiteFiduciarioComponent implements OnInit {

  estadosComite: any;
  data: any[] = [
    {
      fecha: '09/07/2020',
      numero: 'CF_00001',
      estado: 'Desarrollada sin acta',
      id: 0
    },
    {
      fecha: '10/07/2020',
      numero: 'CF_00002',
      estado: 'Desarrollada sin acta',
      id: 0
    }
  ]
  displayedColumns: string[] = ['fecha', 'numero', 'estado', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor ( ) {
  
  }

  ngOnInit(): void {

    this.dataSource = new MatTableDataSource( this.data );
    //getData Tabla sesiones de comite convocadas
    /*
    getComiteGrilla()
      .subscribe( response => {
        let lista: ComiteGrilla[] = response.filter( c => c.estadoComiteCodigo != this.estadosComite.sinConvocatoria )
        this.dataSource = new MatTableDataSource( lista );
      })
    */

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

}

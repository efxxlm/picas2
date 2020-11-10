import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FuenteFinanciacion, FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ViewFlags } from '@angular/compiler/src/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
export interface Contrato {
  id:number,
  fechaFirma: string;
  numeroContrato: string;
  tipoSolicitud: string;
  estado: string;
}

@Component({
  selector: 'app-tabla-registro-presupuestal',
  templateUrl: './tabla-registro-presupuestal.component.html',
  styleUrls: ['./tabla-registro-presupuestal.component.scss']
})
export class TablaRegistroPresupuestalComponent implements OnInit {
  displayedColumns: string[] = [ 'fechaFirma', 'numeroContrato', 'tipoSolicitud', 'estado', 'id'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  constructor( private router: Router, public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService) { }

  ngOnInit(): void {
    let elementos:Contrato[]=[];
    this.disponibilidadServices.GetListGenerarRegistroPresupuestal().subscribe(listas => {
      
      listas.disponibilidadPresupuestal.forEach(element => {
        console.log(element);
        elementos.push({
          id:element.disponibilidadPresupuestalId,
          estado:element.estado,
          fechaFirma:element.fechaFirmaContrato,
          numeroContrato:element.numeroContrato,
          tipoSolicitud:element.tipoSolicitudEspecial});
      });
      this.dataSource = new MatTableDataSource(elementos);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
    
    
  }
  /*
  jflorez, lo cambio por router link
  gestionarDRP(){
    this.router.navigate(['/generarRegistroPresupuestal/gestionarDrp', this.d.id]);
  }
  verDetalle(){
    this.router.navigate(['/generarRegistroPresupuestal/verDetalle', element.id]);
  }*/
}

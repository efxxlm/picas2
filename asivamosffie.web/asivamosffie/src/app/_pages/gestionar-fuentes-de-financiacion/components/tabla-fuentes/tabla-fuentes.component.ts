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


@Component({
  selector: 'app-tabla-fuentes',
  templateUrl: './tabla-fuentes.component.html',
  styleUrls: ['./tabla-fuentes.component.scss']
})
export class TablaFuentesComponent implements OnInit {

  displayedColumns: string[] = [ 'fechaCreacion', 'tipoAportante', 'aportante', 'vigencia', 'fuenteDeRecursos', 'valorAporteFuenteDeRecursos', 'valorAporteEnCuenta', 'estado', 'id'];
  dataSource = new MatTableDataSource();

  listaFF: any[] = [];
  listaNombreAportante: Dominio[] = [];
  listaTipoAportante: Dominio[] =[];
  listaFuenteRecursos: Dominio[] = [];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
                private fuenteFinanciacionService: FuenteFinanciacionService,
                private commonService: CommonService,
                private router: Router,
                public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    
    forkJoin([
      this.fuenteFinanciacionService.listaFuenteFinanciacion(),
      this.commonService.listaNombreAportante(),
      this.commonService.listaTipoAportante(),
      this.commonService.listaFuenteRecursos()
    ]).subscribe( respuesta => {
      this.listaFF = respuesta[0]
      this.listaNombreAportante = respuesta[1];
      this.listaTipoAportante = respuesta[2];
      this.listaFuenteRecursos = respuesta[3];

      this.listaFF.forEach( ff => {
        let nombre = this.listaNombreAportante.find( nom => nom.dominioId == ff.aportante.nombreAportanteId );
        let tipoAportante = this.listaTipoAportante.find( tip => tip.dominioId ==  ff.aportante.tipoAportanteId );
        let fuenteRecursos = this.listaFuenteRecursos.find( fr => fr.codigo == ff.fuenteRecursosCodigo );
        let valorTotalCuenta: number = 0;

        ff.nombreAportante = nombre ? nombre.nombre : '';
        ff.tipoAportante = tipoAportante ? tipoAportante.nombre : ''
        ff.vigencia = ff.vigenciaAporte ? ff.vigenciaAporte.length > 0 ? ff.vigenciaAporte[0].tipoVigenciaCodigo : '': '' ;
        ff.fuenteDeRecursos = fuenteRecursos ? fuenteRecursos.nombre : ''; 

      })
  
      this.dataSource = new MatTableDataSource(this.listaFF);
  
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';

    })

    
  }

  editarFuente(e: number, idTipo: number) {
    console.log(e);
    this.router.navigate(['/registrarFuentes',e,idTipo]);
  }
  eliminarFuente(e: number) {
    this.openDialogSiNo('','¿Está seguro de eliminar este registro?',e)
  }

  controlRecursosFuente(e: number) {
    this.router.navigate(['/gestionarFuentes/controlRecursos',e,0])
  }

  openDialogSiNo(modalTitle: string, modalText: string, e:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result)
      {
        this.eliminarRegistro(e);
      }           
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  eliminarRegistro(e: number){
    this.fuenteFinanciacionService.eliminarFuentesFinanciacion(e).subscribe( resultado => {
      let res = resultado as Respuesta;
      this.openDialog('Fuente Financiacion', res.message);
      this.ngOnInit();
    })
  }

}

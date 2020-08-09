import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProcesoSeleccionService, ProcesoSeleccion, EstadosProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Respuesta, CommonService } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { forkJoin } from 'rxjs';

export interface ProcesosElement {
  id: number;
  tipo: string;
  numero: string;
  fechaSolicitud: string;
  numeroSolicitud: string;
  estadoDelSolicitud: string;
}

const ELEMENT_DATA: ProcesosElement[] = [
  {
    id: 1,
    tipo: 'Selección privada',
    numero: 'SP 0007-2020',
    fechaSolicitud: '01/06/2020',
    numeroSolicitud: '0001',
    estadoDelSolicitud: 'Creada',
  }
];

@Component({
  selector: 'app-tabla-detalle-cronograma',
  templateUrl: './tabla-detalle-cronograma.component.html',
  styleUrls: ['./tabla-detalle-cronograma.component.scss']
})
export class TablaDetalleCronogramaComponent implements OnInit {

  @Input() editMode: any = {};
  idProcesoseleccion: number = 0;
  estadosProcesoSeleccion = EstadosProcesoSeleccion;


  displayedColumns: string[] = [ 'tipo', 'numero', 'fechaSolicitud', 'numeroSolicitud', 'estadoDelSolicitud', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
                private procesoSeleccionService: ProcesoSeleccionService,
                private activatedRoute: ActivatedRoute,
                public dialog: MatDialog,
                private router: Router,
                private commonService: CommonService,

             ) 
  { }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametros => {
      this.idProcesoseleccion = parametros['id'];

      let listaProcesos: ProcesoSeleccion[] = []; 

      forkJoin([

        this.procesoSeleccionService.getProcesoSeleccionById( this.idProcesoseleccion ),
        this.commonService.listaTipoProcesoSeleccion(),
        this.commonService.listaEstadoProcesoSeleccion(),
        this.commonService.listaEtapaProcesoSeleccion(),

      ]).subscribe( respuesta => {

          let proceso = respuesta[0]

          let nombreTipo = respuesta[1].find( p => p.codigo == proceso.tipoProcesoCodigo )
          let nombreEstado = respuesta[2].find( p => p.codigo == proceso.estadoProcesoSeleccionCodigo )
          let nombreEtapa = respuesta[3].find( p => p.codigo == proceso.etapaProcesoSeleccionCodigo )
          
          if (nombreTipo)   proceso.tipoProcesoNombre = nombreTipo.nombre;
          if (nombreEstado) proceso.estadoProcesoSeleccionNombre = nombreEstado.nombre;
          if (nombreEtapa)  proceso.etapaProcesoSeleccionNombre = nombreTipo.nombre;

          listaProcesos.push( proceso );
          this.dataSource = new MatTableDataSource( listaProcesos );

          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
          this.paginator._intl.nextPageLabel = 'Siguiente';
          this.paginator._intl.previousPageLabel = 'Anterior';

      })
    })
  }

  onDetalle(){
    this.editMode.valor = !this.editMode.valor;
    console.log( this.editMode.valor );
  }

  onEnviarSolicitud(  ){
    let proceso: ProcesoSeleccion = {
      procesoSeleccionId: this.idProcesoseleccion,
      estadoProcesoSeleccionCodigo: this.estadosProcesoSeleccion.AperturaEntramite
    }

    this.procesoSeleccionService.changeStateProcesoSeleccion( proceso ).subscribe( respuesta => {
      this.openDialog("Proceso Seleccion", respuesta.message);
      if ( respuesta.code == "200" )
        this.ngOnInit();
    })
  }

  onEliminar(){

    this.openDialogSiNo('','¿Está seguro de eliminar este registro?')
  }

  eliminarRegistro( ){
    this.procesoSeleccionService.deleteProcesoSeleccion( this.idProcesoseleccion ).subscribe( respuesta => {
      let r = respuesta as Respuesta;
       if ( r.code == "200" )
       {
         this.openDialog("Proceso Seleccion", "La información se ha eliminado correctamente,");
         this.router.navigate(['/seleccion']);
       }else
        this.openDialog("Proceso Seleccion", r.message);
    })
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  openDialogSiNo(modalTitle: string, modalText: string ) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result)
      {
        this.eliminarRegistro();
      }           
    });
  }

}

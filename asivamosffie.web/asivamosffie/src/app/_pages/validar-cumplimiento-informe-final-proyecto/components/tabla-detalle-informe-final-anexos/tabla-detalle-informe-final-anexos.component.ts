import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { MatDialog } from '@angular/material/dialog'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'
import { Report } from 'src/app/_interfaces/proyecto-final.model'
import { InformeFinalInterventoriaObservaciones, ListaChequeo } from 'src/app/_interfaces/proyecto-final-anexos.model'
import { ValidarCumplimientoInformeFinalService } from 'src/app/core/_services/validarCumplimientoInformeFinal/validar-cumplimiento-informe-final.service'
import { Router } from '@angular/router'

@Component({
  selector: 'app-tabla-detalle-informe-final-anexos',
  templateUrl: './tabla-detalle-informe-final-anexos.component.html',
  styleUrls: ['./tabla-detalle-informe-final-anexos.component.scss']
})
export class TablaDetalleInformeFinalAnexosComponent implements OnInit {

  @Input() id: number;
  @Input() llaveMen: string;
  @Input() report: Report;
  @Input() existeObservacionInterventoria: boolean;
  existe_historial = false;
  data: InformeFinalInterventoriaObservaciones = {};

  listChequeo: any;
  displayedColumns: string[] = [
    'No',
    'item',
    'tipoAnexoString',
    'Ubicacion',
    'aprobacionCodigoString',
  ];

  ELEMENT_DATA : ListaChequeo[] = [];

  dataSource = new MatTableDataSource<ListaChequeo>(this.ELEMENT_DATA);


  @ViewChild(MatPaginator) paginator: MatPaginator
  @ViewChild(MatSort) sort: MatSort
  constructor(
    public dialog: MatDialog,
    private validarCumplimientoInformeFinalProyectoService: ValidarCumplimientoInformeFinalService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getInformeFinalListaChequeoByInformeFinalId(this.id);
  }

  getInformeFinalListaChequeoByInformeFinalId (id:number) {
    this.validarCumplimientoInformeFinalProyectoService.getInformeFinalListaChequeoByInformeFinalId(id)
    .subscribe(listChequeo => {
      this.dataSource.data = listChequeo as ListaChequeo[];
      this.listChequeo = listChequeo;
    });
    if(this.report != null){
      if(this.report.proyecto.informeFinal[0].observacionVigenteInformeFinalInterventoriaNovedades != null){
        const observaciones = this.report.proyecto.informeFinal[0].observacionVigenteInformeFinalInterventoriaNovedades.observaciones;
        if(observaciones != null && observaciones != 'undefined'){
          this.data.observaciones = observaciones;
        }else{
          this.data.observaciones = "";
        }
        this.data.fechaCreacion = this.report.proyecto.informeFinal[0].observacionVigenteInformeFinalInterventoriaNovedades.fechaCreacion;
      }
      else if(this.report.proyecto.informeFinal[0].historialObsInformeFinalInterventoriaNovedades.length > 0){
        this.existe_historial = true;
        this.data.fechaCreacion = this.report.proyecto.informeFinal[0].historialObsInformeFinalInterventoriaNovedades[0].fechaCreacion;
        const observaciones = this.report.proyecto.informeFinal[0].historialObsInformeFinalInterventoriaNovedades[0].observaciones
        if(observaciones != null && observaciones != 'undefined'){
          this.data.observaciones = observaciones;
        }else{
          this.data.observaciones = "";
        }
      }
    }
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort
    this.dataSource.paginator = this.paginator
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página'
    this.paginator._intl.nextPageLabel = 'Siguiente'
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length
      }
      length = Math.max(length, 0)
      const startIndex = page * pageSize
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length
    }
    this.paginator._intl.previousPageLabel = 'Anterior'
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }
}


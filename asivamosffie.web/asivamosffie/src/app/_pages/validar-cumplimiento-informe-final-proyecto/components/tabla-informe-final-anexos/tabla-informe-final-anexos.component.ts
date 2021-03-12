import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { MatDialog } from '@angular/material/dialog'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'
import { ListaChequeo } from 'src/app/_interfaces/proyecto-final-anexos.model'
import { FormGroup } from '@angular/forms'
import { ValidarCumplimientoInformeFinalService } from 'src/app/core/_services/validarCumplimientoInformeFinal/validar-cumplimiento-informe-final.service'
import { Report } from 'src/app/_interfaces/proyecto-final.model'

@Component({
  selector: 'app-tabla-informe-final-anexos',
  templateUrl: './tabla-informe-final-anexos.component.html',
  styleUrls: ['./tabla-informe-final-anexos.component.scss']
})
export class TablaInformeFinalAnexosComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : ListaChequeo[] = [];
  @Input() id: number;
  @Input() llaveMen: string; 
  @Input() report: Report;

  listChequeo: any;
  displayedColumns: string[] = [
    'No',
    'item',
    'tipoAnexoString',
    'Ubicacion',
    'aprobacionCodigoString',
  ];
  addressForm: FormGroup;
  dataSource = new MatTableDataSource<ListaChequeo>(this.ELEMENT_DATA);
  semaforo = false;
  estaEditando = false;
  noGuardado=false;

  @ViewChild(MatPaginator) paginator: MatPaginator
  @ViewChild(MatSort) sort: MatSort
  constructor(
    private validarCumplimientoInformeFinalProyectoService: ValidarCumplimientoInformeFinalService,
    public dialog: MatDialog
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

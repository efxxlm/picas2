import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { MatDialog } from '@angular/material/dialog'
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';
import { VerificarInformeFinalService } from 'src/app/core/_services/verificarInformeFinal/verificar-informe-final.service'
import { ListaChequeo } from 'src/app/_interfaces/proyecto-final-anexos.model'
import { InformeFinal, InformeFinalAnexo, InformeFinalInterventoria, InformeFinalInterventoriaObservaciones } from 'src/app/_interfaces/informe-final';
import { Respuesta } from 'src/app/core/_services/common/common.service'
import { Router } from '@angular/router'

@Component({
  selector: 'app-tabla-informe-final-anexos',
  templateUrl: './tabla-informe-final-anexos.component.html',
  styleUrls: ['./tabla-informe-final-anexos.component.scss']
})
export class TablaInformeFinalAnexosComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : ListaChequeo[] = [];
  @Input() id: number;
  @Input() llaveMen: string;
  listChequeo: any;
  displayedColumns: string[] = [
    'No',
    'item',
    'calificacionCodigoString',
    'tipoAnexoString',
    'Ubicacion',
    'validacionCodigo',
    'id'
  ];
  estadoValidacion = '0';
  registroCompletoValidacion = false;
  addressForm: FormGroup;
  informeFinalObservacion : InformeFinalInterventoriaObservaciones[] = [];
  dataSource = new MatTableDataSource<ListaChequeo>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator
  @ViewChild(MatSort) sort: MatSort
  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private validarInformeFinalService: VerificarInformeFinalService,
    private router: Router
  ) {}

  estaEditando = false;
  noGuardado=false;
  
  ngOnDestroy(): void {
    if ( this.noGuardado===true) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        // console.log(`Dialog result: ${result}`);
        if(result === true)
        {
            this.onSubmit();          
        }           
      });
    }
  };

  estadoArray = [
    { name: 'Cumple', value: 1 },
    { name: 'No cumple', value: 2 },
    { name: 'No aplica', value: 3 }
  ]

  ngOnInit(): void {
    this.getInformeFinalListaChequeoByInformeFinalId(this.id);
  }

  getInformeFinalListaChequeoByInformeFinalId (id:number) {
    this.validarInformeFinalService.getInformeFinalListaChequeoByInformeFinalId(id)
    .subscribe(listChequeo => {
      if(listChequeo != null){
        this.estadoValidacion = listChequeo[0].estadoValidacion;
        this.registroCompletoValidacion = listChequeo[0].registroCompletoValidacion;
      }
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

  openDialogObservaciones(informe:any) {
    this.informeFinalObservacion = null;
    this.dataSource.data.forEach(control => {
      if ( informe !== null && informe.informeFinalInterventoriaId === control.informeFinalInterventoriaId ) {
        if(control.informeFinalInterventoriaObservaciones != null && control.informeFinalInterventoriaObservaciones.length > 0){
          this.informeFinalObservacion = control.informeFinalInterventoriaObservaciones;
        }
        return;
      }
    });
    let dialogRef = this.dialog.open(DialogObservacionesComponent, {
      width: '70em',
      data: {
        informe: informe,
        llaveMen: this.llaveMen,
        informeFinalObservacion: this.informeFinalObservacion,
      },
      id:'dialogObservaciones'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.dataSource.data.forEach(control => {
        if ( result !== null && result.id === control.informeFinalInterventoriaId ) {
          control.informeFinalInterventoriaObservaciones = [];
          const informeFinalInterventoriaObservaciones: InformeFinalInterventoriaObservaciones = {
            informeFinalInterventoriaObservacionesId:result.observaciones.informeFinalInterventoriaObservacionesId,
            informeFinalInterventoriaId:result.observaciones.informeFinalInterventoriaId,
            observaciones: result.observaciones.observaciones,
            esSupervision: result.observaciones.esSupervision,
            esCalificacion: result.observaciones.esCalificacion
          };
          control.tieneObservacionNoCumple = true;
          control.informeFinalInterventoriaObservaciones.push(informeFinalInterventoriaObservaciones);
          return;
        }
      });
      return;
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.noGuardado=false;
    //recorre el datasource y crea modelo
    const listaInformeFinalInterventoria = [] as InformeFinal;
    listaInformeFinalInterventoria.informeFinalInterventoria = [];
    this.dataSource.data.forEach(control => {
        const informeFinalInterventoria: InformeFinalInterventoria = {
          informeFinalInterventoriaId: control.informeFinalInterventoriaId,
          informeFinalId: control.informeFinalId,
          calificacionCodigo: control.calificacionCodigo,
          informeFinalListaChequeoId: control.informeFinalListaChequeoId,
          informeFinalAnexo: null,
          informeFinalInterventoriaObservaciones: control.informeFinalInterventoriaObservaciones,
          validacionCodigo: control.validacionCodigo
        };
        listaInformeFinalInterventoria.informeFinalInterventoria.push(informeFinalInterventoria);
    });
    const informeFinal: InformeFinal = {
      informeFinalId: this.dataSource.data[0].informeFinalId,
      //proyectoId: Number(this.id),
      informeFinalInterventoria: listaInformeFinalInterventoria.informeFinalInterventoria,
    };
    this.updateStateValidateInformeFinalInterventoriaByInformeFinal(informeFinal);
    this.router.navigate(['/verificarInformeFinalProyecto']);
  }

  updateStateValidateInformeFinalInterventoriaByInformeFinal( informeFinal: any ) {
    this.validarInformeFinalService.updateStateValidateInformeFinalInterventoriaByInformeFinal(informeFinal)
    .subscribe((respuesta: Respuesta) => {
        console.log(respuesta);
        this.openDialog('', respuesta.message);
        //this.ngOnInit();
        return;
      },
      err => {
        this.openDialog('', err.message);
        //this.ngOnInit();
        return;
      });
  }

  changeState(){
    this.noGuardado = true;
  }

}

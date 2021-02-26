import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { InformeFinalInterventoria } from 'src/app/_interfaces/proyecto-final-anexos.model';

@Component({
  selector: 'app-tabla-observaciones-recibo-satisfaccion',
  templateUrl: './tabla-observaciones-recibo-satisfaccion.component.html',
  styleUrls: ['./tabla-observaciones-recibo-satisfaccion.component.scss']
})
export class TablaObservacionesReciboSatisfaccionComponent implements OnInit, AfterViewInit {
  @Input() data: any;
  ELEMENT_DATA : InformeFinalInterventoria[] = [];
  anexos: any[];
  dataSource = new MatTableDataSource<InformeFinalInterventoria>(this.ELEMENT_DATA);
  displayedColumns: string[] = ['fecha', 'observaciones']

  observacionesForm = this.fb.group({
    observaciones: [null, Validators.required],
    fechaCreacion: [null, Validators.required],   
  })

  constructor(
    private fb: FormBuilder,
  ) {}
  
  @ViewChild(MatPaginator) paginator: MatPaginator
  @ViewChild(MatSort) sort: MatSort

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {
    this.observacionesForm = this.fb.group({
      observaciones: [null, Validators.required],
      fechaCreacion: [null,Validators.required],
    });
    if(this.data.historialInformeFinalInterventoriaObservaciones != null){
      if(this.data.historialInformeFinalInterventoriaObservaciones.length > 0){
        this.observacionesForm.patchValue(this.data.historialInformeFinalInterventoriaObservaciones);
        this.dataSource.data = this.data.historialInformeFinalInterventoriaObservaciones as InformeFinalInterventoria[];
        this.anexos = this.data.historialInformeFinalInterventoriaObservaciones;
      }      
    }

    if(this.data.observacionVigenteSupervisor != null){
      this.observacionesForm.patchValue(this.data.observacionVigenteSupervisor)
    }
    if(this.data.historialInformeFinalInterventoriaObservaciones != null){
      if(this.data.historialInformeFinalInterventoriaObservaciones.length > 0){
        this.observacionesForm.patchValue(this.data.historialInformeFinalInterventoriaObservaciones);
        this.dataSource.data = this.data.historialInformeFinalInterventoriaObservaciones as InformeFinalInterventoria[];
        this.anexos = this.data.historialInformeFinalInterventoriaObservaciones;
      }      
    }
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort
    this.dataSource.paginator = this.paginator
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página'
    this.paginator._intl.nextPageLabel = 'Siguiente'
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      return (page + 1).toString() + ' de ' + length.toString()
    }
    this.paginator._intl.previousPageLabel = 'Anterior'
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value
    this.dataSource.filter = filterValue.trim().toLowerCase()

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage()
    }
  }
}

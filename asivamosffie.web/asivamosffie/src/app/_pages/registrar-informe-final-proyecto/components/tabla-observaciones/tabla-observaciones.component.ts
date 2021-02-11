import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { InformeFinalInterventoria } from 'src/app/_interfaces/proyecto-final-anexos.model';

const ELEMENT_DATA = [
  {
    fecha: '21/06/2020',
    observaciones: 'LJ776554'
  }
]
@Component({
  selector: 'app-tabla-observaciones',
  templateUrl: './tabla-observaciones.component.html',
  styleUrls: ['./tabla-observaciones.component.scss']
})
export class TablaObservacionesComponent implements OnInit, AfterViewInit {
  @Input() data: any;
  ELEMENT_DATA : InformeFinalInterventoria[] = [];
  anexos: any[];
  dataSourceTest = new MatTableDataSource<InformeFinalInterventoria>(this.ELEMENT_DATA);
  displayedColumns: string[] = ['fecha', 'observaciones']
  dataSource = new MatTableDataSource(ELEMENT_DATA)
  observacionesForm: FormGroup;

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
    if(this.data.informeFinalInterventoriaObservaciones != null){
      if(this.data.informeFinalInterventoriaObservaciones.length > 0){
        this.observacionesForm.patchValue(this.data.informeFinalInterventoriaObservaciones);
        this.dataSourceTest.data = this.data.informeFinalInterventoriaObservaciones as InformeFinalInterventoria[];
        this.anexos = this.data.informeFinalInterventoriaObservaciones;
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

import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-actualizacion-poliza-gtlc',
  templateUrl: './actualizacion-poliza-gtlc.component.html',
  styleUrls: ['./actualizacion-poliza-gtlc.component.scss']
})
export class ActualizacionPolizaGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'polizaYSeguros',
    'responsableAprobacion'
  ];
  dataTable: any[] = [
    {
      polizaYSeguros: 'Buen manejo y correcta inversión del anticipo',
      responsableAprobacion: 'Andres Nikolai Montealegre Rojas'
    },
    {
      polizaYSeguros: 'Garantía de estabilidad y calidad de la obra',
      responsableAprobacion: 'Andres Nikolai Montealegre Rojas'
    }
  ]
  addressForm = this.fb.group({});
  editorStyle = {
    height: '45px',
    overflow: 'auto'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  estaEditando = false;
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.loadDataSource();
  }
  
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
  crearFormulario() {
    return this.fb.group({
      tieneObservaciones: [null, Validators.required],
      observaciones:[null, Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    console.log(e.editor.getLength()+" "+n);
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }
  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    console.log(this.addressForm.value);
  }
}

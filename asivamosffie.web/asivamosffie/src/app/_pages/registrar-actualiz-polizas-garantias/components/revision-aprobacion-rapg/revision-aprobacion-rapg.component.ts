import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-revision-aprobacion-rapg',
  templateUrl: './revision-aprobacion-rapg.component.html',
  styleUrls: ['./revision-aprobacion-rapg.component.scss']
})
export class RevisionAprobacionRapgComponent implements OnInit {

  //Form
  addressForm = this.fb.group({
    fechaRevision: [null, Validators.required],
    estadoRevision: [null, Validators.required],
    fechaAprob: ['', Validators.required],
    responsableAprob: ['', Validators.required],
    observacionesGenerales: ['']
  });
  estaEditando = false;

  //parametricas
  estadoArray = [];
  listaUsuarios: any[] = [];
  constructor(private fb: FormBuilder,private common: CommonService) { 
    this.common.getUsuariosByPerfil(10).subscribe(resp => {
      this.listaUsuarios = resp;
    });
    this.common.listaEstadoRevision().subscribe(resp=>{
      this.estadoArray=resp;
    });
  }

  ngOnInit(): void {
  }
    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }
  
    maxLength(e: any, n: number) {
      if (e.editor.getLength() > n) {
        e.editor.deleteText(n, e.editor.getLength());
      }
    }
  
    textoLimpio(texto: string) {
      let saltosDeLinea = 0;
      saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
      saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');
  
      if ( texto ){
        const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
        return textolimpio.length + saltosDeLinea;
      }
    }
  
    private contarSaltosDeLinea(cadena: string, subcadena: string) {
      let contadorConcurrencias = 0;
      let posicion = 0;
      while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
        ++contadorConcurrencias;
        posicion += subcadena.length;
      }
      return contadorConcurrencias;
    }
  onSubmit() {
    this.estaEditando = true;
  }
}

<script setup>
import { ref } from 'vue'
</script>

<template>
    <footer>
        <div
            class="footer-first-row flex flex-col-reverse items-center gap-10 gap-y-20 py-24 pt-20 md:flex md:flex-row md:flex-wrap md:justify-between md:gap-10 md:py-24 lg:flex-nowrap">
            <div class="md:basis-full lg:basis-auto">
                <img height="165" dam-content-type="Image" alt="Logo" width="150" filename="script-added-filename.svg"
                    thumbnailsrc="https://uoi-q-001.sitecorecontenthub.cloud/api/gateway/41299/thumbnail"
                    dam-id="VER0W8EoRzSL6hB53QJOig" loading="lazy" class="mx-auto md:mx-0"
                    src="https://dev-content-hub.uidaho.edu/api/public/content/93bfa35085204148badd8e0054b7557f?v=80c9bac3" />
            </div>
            <div class="space-y-7">
                <address class="space-y-1 text-md text-brand-white">
                    <p class="text-xl font-extrabold">University of Idaho</p>
                    <p>875 Perimeter Drive, Moscow, ID 83844</p>
                    <p>208-885-6111</p>
                    <p>
                        <a href="mailto:info@uidaho.edu" title="General contact email for U of I.">
                            info@uidaho.edu
                        </a>
                    </p>
                </address>
            </div>
            <div class="flex flex-col justify-between md:flex-row md:items-center">
                <a title="" target=""
                    class="focus-visible:box-highlight group/link group my-4 flex min-h-52 w-full max-w-[420px] flex-col justify-between gap-6 border-2 p-6 bg-brand-black text-brand-white border-gold"
                    data-component="SimpleCTA" href="/give">
                    <span>
                        <span class="text-4xl font-black">Support a Vandal</span>
                    </span>
                </a>
            </div>
        </div>
        <div class="bg-gold py-8 copyright">
            <div class="container">
                <div class="flex flex-row-reverse flex-wrap justify-center gap-10 md:flex-nowrap md:justify-between">
                    <p class="text-nowrap text-base font-semibold">
                        © 2026
                        <span>University of Idaho</span>
                    </p>
                </div>
            </div>
        </div>
    </footer>
</template>